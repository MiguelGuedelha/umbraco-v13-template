using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var sqlServer = builder
    .AddSqlServer("sqlserver")
    .WithDataVolume("umbracoplayground-mssql-data")
    .WithVolume("umbracoplayground-mssql-log", "/var/opt/mssql/log")
    .WithVolume("umbracoplayground-mssql-secrets", "/var/opt/mssql/secrets")
    .WithContainerRuntimeArgs("--user", "root");

var umbracoDb = sqlServer.AddDatabase("umbracoDbDSN", "umbraco-cms");

var cms = builder.AddProject<Projects.UmbracoTemplate_Cms>("cms")
    .WithExternalHttpEndpoints()
    .WithReference(umbracoDb);

builder.Build().Run();