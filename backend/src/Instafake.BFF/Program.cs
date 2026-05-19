using Instafake.BFF.Config;
using Instafake.BFF.Db.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

var idp = builder.Configuration.GetRequiredSection("IdentityProviders");
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

services.AddAuthorization();

services.AddDbContext<OpenIddictDbContext>(cfg =>
{
    cfg.UseInMemoryDatabase("OpenIddict");
    cfg.UseOpenIddict();
});

services.AddOpenIddict()
    .AddCore(cfg =>
    {
        cfg.UseEntityFrameworkCore()
            .UseDbContext<OpenIddictDbContext>();
    })
    .AddClient(options =>
    {
        options.AllowAuthorizationCodeFlow();

        options.AddDevelopmentSigningCertificate()
            .AddDevelopmentEncryptionCertificate();

        options.UseAspNetCore()
            .EnableRedirectionEndpointPassthrough();

        options.UseWebProviders()
            .AddGitHub(cfg =>
            {
                cfg.SetClientId(idp.GetValue<string>("GitHub:ClientId"));
                cfg.SetClientSecret(idp.GetValue<string>("GitHub:ClientSecret"));
                cfg.SetRedirectUri(idp.GetValue<string>("GitHub:CallbackPath"));
            });
    });

services.Configure<FrontendConfig>(builder.Configuration.GetRequiredSection("Frontend"));

builder.AddServiceDefaults();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();