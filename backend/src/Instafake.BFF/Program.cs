using Auth0.AspNetCore.Authentication;
using Instafake.BFF.Config;
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
});

services.AddAuthorization();

services.Configure<FrontendConfig>(builder.Configuration.GetRequiredSection("Frontend"));

builder.AddServiceDefaults();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();