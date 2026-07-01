using Confluent.Kafka;
using Instafake.IdentityEventsIngress.Events;
using Instafake.ServiceDefaults;
using JasperFx.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using Wolverine;
using Wolverine.Kafka;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);
builder.UseWolverine(options =>
{
    options.UseRuntimeCompilation();
    options.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());

    options.PersistMessagesWithPostgresql(builder.Configuration.GetConnectionString("identity-events-ingress-api-db"));

    options.Policies.AutoApplyTransactions();

    var kafkaHost = builder.Configuration.GetRequiredSection("Kafka:Host").Get<ConsumerConfig>()!;
    options.UseKafka(kafkaHost.BootstrapServers)
        .AutoProvision();

    var kafkaDefaultProducer = builder.Configuration.GetSection("Kafka:Producers:Default").Get<ProducerConfig>() ?? new ProducerConfig();

    var usersProducer = new ProducerConfig(kafkaDefaultProducer);
    builder.Configuration.GetSection("Kafka:Producers:Users").Bind(usersProducer);

    options.PublishMessage<UserCreatedEvent>()
        .ToKafkaTopic("users")
        .UseDurableOutbox();
});
builder.Host.UseResourceSetupOnStartup();

builder.AddServiceDefaults();
builder.Services.AddControllers();

var jwtBearer = builder.Configuration.GetSection("JwtBearer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = jwtBearer.GetValue<string>("Authority");
        cfg.Audience = jwtBearer.GetValue<string>("Audience");
        cfg.SaveToken = false;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
