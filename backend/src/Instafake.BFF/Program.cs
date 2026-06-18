using Auth0.AspNetCore.Authentication;
using Duende.AccessTokenManagement;
using Duende.AccessTokenManagement.OpenIdConnect;
using Instafake.BFF.Config;
using Instafake.BFF.ExceptionHandlers;
using Instafake.BFF.ServicesProtos.Post;
using Instafake.ServiceDefaults;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

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

services.AddAuthorization();

services.AddOpenIdConnectAccessTokenManagement();

/*
builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Domain = ".dev.localhost";
});
*/

var grpcServicesConfig = builder.Configuration.GetRequiredSection("GrpcServices");
var postsGrpcServiceConfig = grpcServicesConfig.GetRequiredSection("Posts");
services.AddGrpcClient<Poster.PosterClient>(cfg =>
{
    cfg.Address = postsGrpcServiceConfig.GetValue<Uri>("Url");
}).AddUserAccessTokenHandler().AddDefaultAccessTokenResiliency();

var frontendConfig = new FrontendConfig();
var frontendSection = builder.Configuration.GetRequiredSection("Frontend");
frontendSection.Bind(frontendConfig);

services.Configure<FrontendConfig>(frontendSection);

services.AddCors(cfg =>
{
    cfg.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendConfig.Url.AbsoluteUri);
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
    });
});

services.AddExceptionHandler<GrpcExceptionHandler>();
services.AddProblemDetails();

services.AddHttpForwarderWithServiceDiscovery();
services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetRequiredSection("ReverseProxy"))
    .AddTransforms(builder =>
    {
        builder.AddRequestTransform(async (ctx) =>
        {
            var userToken = await ctx.HttpContext.GetUserAccessTokenAsync(ct: ctx.CancellationToken);
            if (!userToken.Succeeded)
                return;

            ctx.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token.AccessToken);
        });
    }).AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.MapControllers();

app.MapReverseProxy();

await app.RunAsync();