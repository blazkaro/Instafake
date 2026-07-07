using Confluent.Kafka;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Instafake.Notifications.DbContexts;
using Instafake.Notifications.Endpoints;
using Instafake.ServiceDefaults;
using JasperFx.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);
builder.UseWolverine(options =>
{
    options.UseRuntimeCompilation();
    options.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());

    options.PersistMessagesWithPostgresql(builder.Configuration.GetConnectionString("notifications-db"));
    options.UseEntityFrameworkCoreWolverineManagedMigrations();

    options.Policies.AutoApplyTransactions();
    options.UseEntityFrameworkCoreTransactions();

    var kafkaHost = builder.Configuration.GetRequiredSection("Kafka:Host").Get<ConsumerConfig>()!;
    options.UseKafka(kafkaHost.BootstrapServers)
        .AutoProvision();

    var kafkaDefaultConsumer = builder.Configuration.GetSection("Kafka:Producers:Default").Get<ProducerConfig>() ?? new ProducerConfig();

    var notificationsConsumer = new ConsumerConfig(kafkaDefaultConsumer);
    builder.Configuration.GetSection("Kafka:Producers:Notifications").Bind(notificationsConsumer);

    options.ListenToKafkaTopic("post-notifications")
        .ConfigureConsumer(cfg =>
        {
            foreach (var keyPair in notificationsConsumer)
            {
                cfg.Set(keyPair.Key, keyPair.Value);
            }
        })
        .UseDurableInbox();
});
builder.Host.UseResourceSetupOnStartup();

builder.Services.AddDbContextWithWolverineIntegration<DevicesDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration.GetConnectionString("notifications-db"))
        .UseSnakeCaseNamingConvention(); // wolverine manages migrations, use for consistent naming
});

var jwtBearer = builder.Configuration.GetSection("JwtBearer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = jwtBearer.GetValue<string>("Authority");
        cfg.Audience = jwtBearer.GetValue<string>("Audience");
        cfg.SaveToken = false;
    });

builder.Services.AddAuthorization();

builder.AddServiceDefaults();
var app = builder.Build();

var firebaseSection = builder.Configuration.GetSection("FirebaseCredentials");
var configDict = firebaseSection.Get<Dictionary<string, object>>();
if (configDict != null && configDict.Count > 0)
{
    string jsonString = JsonSerializer.Serialize(configDict);
    var credential = CredentialFactory.FromJson<ServiceAccountCredential>(jsonString)
                                      .ToGoogleCredential();

    FirebaseApp.Create(new AppOptions()
    {
        Credential = credential
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/devices", CreateDeviceEndpoint.Create)
    .RequireAuthorization(policy => policy.RequireAuthenticatedUser());

app.MapDefaultEndpoints();

await app.RunAsync();
