using Umbraco.Community.SecretManager.Site.Compose;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var secretClient = builder.ConfigureKeyVault();
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Configure(secretClient)
    .Build();

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
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
