var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Instafake_BFF>("instafake-bff");

builder.AddProject<Projects.Instafake_Posts_Api>("instafake-posts-api");

builder.Build().Run();
