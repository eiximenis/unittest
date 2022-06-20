using Bank.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

new Startup().ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();
new Startup().Configure(app);
app.Run();
