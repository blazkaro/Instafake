using Instafake.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithDataVolume();

var npgsql = builder.AddPostgres("postgres")
    .WithHostPort(5432) // static to preserve the same connection string when restarted
    .WithDataVolume();

var mssql = builder.AddSqlServer("mssql")
    .WithHostPort(1433) // static to preserve the same connection string when restarted
    .WithDataVolume();

var postsApiDb = mssql.AddDatabase("posts-api-db");
var postsApi = builder.AddProject<Projects.Instafake_Posts_Api>("instafake-posts-api")
    .WithReference(kafka)
    .WithReference(postsApiDb)
    .WaitFor(kafka)
    .WaitFor(postsApiDb)
    .WithKafkaHostEnvironment(kafka);

postsApi.AddEFMigrations("posts-api-migrations", "PostsDbContext")
    .WithMigrationsProject("../Instafake.Posts.Infrastructure")
    .WithReference(postsApi)
    .WaitFor(postsApi)
    .RunDatabaseUpdateOnStart();

var identityEventsIngressDb = npgsql.AddDatabase("identity-events-ingress-api-db");
var identityEventsIngress = builder.AddProject<Projects.Instafake_IdentityEventsIngress>("instafake-identityeventsingress")
    .WithExternalHttpEndpoints()
    .WithReference(kafka)
    .WithReference(identityEventsIngressDb)
    .WaitFor(kafka)
    .WaitFor(identityEventsIngressDb)
    .WithKafkaHostEnvironment(kafka);

builder.AddDevTunnel("tunnel")
    .WithReference(identityEventsIngress)
    .WithAnonymousAccess();

// Minio setup, for dev only
var s3 = builder.AddMinioContainer("minio")
    .WithImage("firstfinger/minio", "latest")
    .WithArgs(ctx => ctx.Args.Clear()) // necessary for that custom image (Default Minio integration appends commands which break this image)
    .WithHttpEndpoint(targetPort: 9002, name: "admin-ui")
    .WithHttpsEndpoint(port: 61974, targetPort: 9000, name: "http") // static (otherwise for services it would look like we change storage provider every restart); "http" name required for proper override
    .WithDataVolume();

var multimediaApi = builder.AddProject<Projects.Instafake_Multimedia>("instafake-multimedia")
    .WithReference(s3)
    .WaitFor(s3)
    .WithEnvironment(ctx =>
    {
        ctx.EnvironmentVariables.TryAdd("AWS_S3__ACCESSKEY", ctx.EnvironmentVariables.GetValueOrDefault("MINIO_ACCESSKEY") ?? throw new InvalidOperationException("Minio access key not provided"));
        ctx.EnvironmentVariables.TryAdd("AWS_S3__SECRETKEY", ctx.EnvironmentVariables.GetValueOrDefault("MINIO_SECRETKEY") ?? throw new InvalidOperationException("Minio secret key not provided"));
        ctx.EnvironmentVariables.TryAdd("AWS_S3__SERVICEURL", ctx.EnvironmentVariables.GetValueOrDefault("MINIO_URI") ?? throw new InvalidOperationException("Minio service url not provided"));

        ctx.EnvironmentVariables.TryAdd("STORAGE__PUBLICURL", ctx.EnvironmentVariables.GetValueOrDefault("MINIO_URI") ?? throw new InvalidOperationException("Minio service url not provided"));

        foreach (var env in ctx.EnvironmentVariables.Where(p => p.Key.Contains("Minio", StringComparison.OrdinalIgnoreCase)))
        {
            ctx.EnvironmentVariables.Remove(env.Key);
        }
    });

var profilesApiDb = npgsql.AddDatabase("profiles-api-db");
var profilesApi = builder.AddProject<Projects.Instafake_Profiles_Api>("instafake-profiles-api")
    .WithReference(kafka)
    .WithReference(profilesApiDb)
    .WaitFor(kafka)
    .WaitFor(profilesApiDb)
    .WithKafkaHostEnvironment(kafka);

// manages migrations on its own
/*
profilesApi.AddEFMigrations("profiles-api-migrations", "ProfilesDbContext")
    .WithMigrationsProject("../Instafake.Profiles.Infrastructure")
    .WithReference(profilesApi)
    .WaitFor(profilesApi)
    .RunDatabaseUpdateOnStart();
*/
var frontend = builder.AddViteApp("instafake-frontend", "../../../frontend", "start");

bool frontendHttps = frontend.Resource.Annotations.OfType<EndpointAnnotation>().Any(p => p.Name == "https");
var bff = builder.AddProject<Projects.Instafake_BFF>("instafake-bff")
    .WithExternalHttpEndpoints()
    .WithReference(frontend)
    .WaitFor(frontend)
    // static host ports for Identity provider integration
    .WithHttpsEndpoint(port: 7153)
    .WithHttpEndpoint(port: 5242)
    .WithEnvironment("Frontend__Url", frontendHttps ? frontend.GetEndpoint("https") : frontend.GetEndpoint("http"))
    .WithReference(postsApi)
    .WaitFor(postsApi)
    .WithReference(multimediaApi)
    .WaitFor(multimediaApi);

frontend.WithEnvironment("VITE_BFF_URL", bff.GetEndpoint("https"));

await builder.Build().RunAsync();
