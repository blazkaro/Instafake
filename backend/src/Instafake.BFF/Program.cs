using Auth0.AspNetCore.Authentication;
using Instafake.BFF.Config;
using Instafake.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

var idp = builder.Configuration.GetRequiredSection("IdP");
services.AddAuth0WebAppAuthentication(cfg =>
{
    cfg.Domain = idp.GetValue<string>("Domain");
    cfg.ClientId = idp.GetValue<string>("ClientId");
    cfg.ClientSecret = idp.GetValue<string>("ClientSecret");
    cfg.CallbackPath = idp.GetValue<string>("CallbackPath");
    cfg.ResponseType = OpenIdConnectResponseType.Code;
    cfg.Scope = "openid profile offline_access";
}).WithAccessToken(cfg =>
{
    cfg.Audience = idp.GetValue<string>("Audience");
    cfg.UseRefreshTokens = true;
});

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Domain = ".dev.localhost";
});

var frontend = builder.Configuration.GetRequiredSection("Frontend");
services.Configure<FrontendConfig>(frontend);

services.AddCors(cfg =>
{
    cfg.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontend.GetValue<string>("Uri"));
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
    });
});

services.AddAuthorization();
builder.AddServiceDefaults();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();