using Bank2.Api;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup();

// Add services to the container.

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.Run();
