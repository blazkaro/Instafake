using Amazon.Runtime;
using Amazon.S3;
using Instafake.Multimedia;
using Instafake.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    bool isDev = builder.Environment.IsDevelopment();
    var s3Config = new AmazonS3Config
    {
        ForcePathStyle = isDev
    };

    var awsOptions = builder.Configuration.GetRequiredSection("AWS_S3");
    awsOptions.Bind(s3Config);

    var credentials = new BasicAWSCredentials(awsOptions.GetValue<string>("AccessKey"), awsOptions.GetValue<string>("SecretKey"));
    return new AmazonS3Client(credentials, s3Config);
});

builder.Services.Configure<StorageConfig>(builder.Configuration.GetRequiredSection("Storage"));

var jwtBearer = builder.Configuration.GetSection("JwtBearer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = jwtBearer.GetValue<string>("Authority");
        cfg.Audience = jwtBearer.GetValue<string>("Audience");
        cfg.SaveToken = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapControllers();

await app.RunAsync();