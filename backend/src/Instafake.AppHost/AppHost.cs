var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Instafake_BFF>("instafake-bff");

builder.Build().Run();
