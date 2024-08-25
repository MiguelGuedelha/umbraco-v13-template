using UmbracoTemplate.Cms.Features.Navigation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

if (bool.TryParse(Environment.GetEnvironmentVariable("USE_USER_SECRETS"), out var useUserSecrets) 
    && useUserSecrets)
{
    builder.Configuration.AddUserSecrets<Program>();
}
    
builder.AddServiceDefaults();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

app.MapDefaultEndpoints();

//Custom endpoints
var apiGroup = app.MapGroup("api");
apiGroup.MapNavigationEndpoints();

await app.RunAsync();
