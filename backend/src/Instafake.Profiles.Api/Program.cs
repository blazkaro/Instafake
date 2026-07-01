using Instafake.Profiles.Application;
using Instafake.Profiles.Infrastructure;
using Instafake.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureInfrastructure(builder.Configuration);

var services = builder.Services;

var jwtBearer = builder.Configuration.GetSection("JwtBearer");
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = jwtBearer.GetValue<string>("Authority");
        cfg.Audience = jwtBearer.GetValue<string>("Audience");
        cfg.SaveToken = false;
    });

services.AddAuthorization();

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);

services.AddControllers();

builder.AddServiceDefaults();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

await app.RunAsync();
