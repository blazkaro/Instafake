using Instafake.Posts.Api.Services;
using Instafake.Posts.Api.Workers;
using Instafake.Posts.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var jwtBearer = builder.Configuration.GetSection("JwtBearer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = jwtBearer.GetValue<string>("Authority");
        cfg.Audience = jwtBearer.GetValue<string>("Audience");
        cfg.SaveToken = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<AuthorEventsWorker>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<PosterService>()
    .RequireAuthorization(policy =>
    {
        policy.RequireAuthenticatedUser();
    });

await app.RunAsync();
