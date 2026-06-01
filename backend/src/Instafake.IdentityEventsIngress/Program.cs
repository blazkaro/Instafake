using Confluent.Kafka;
using Instafake.IdentityEventsIngress.Events;
using Instafake.IdentityEventsIngress.Retry;
using Instafake.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var config = builder.Configuration.GetRequiredSection("Kafka").Get<ProducerConfig>();
    return new ProducerBuilder<string, string>(config).Build();
});

builder.Services.AddSingleton<RetryQueue<UserCreatedEvent>>();
builder.Services.AddSingleton<IEventPublisher<UserCreatedEvent>, KafkaEventPublisher<UserCreatedEvent>>();

builder.Services.AddHostedService<KafkaRetryWorker<UserCreatedEvent>>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
