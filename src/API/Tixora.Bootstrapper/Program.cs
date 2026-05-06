using Events.Api;
using Scalar.AspNetCore;
using Serilog;
using Tixora.Bootstrapper.Extensions;
using Tixora.Shared.Application;
using Tixora.Shared.Infrastructure;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

ILogger logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
Log.Logger = logger;

builder.Services.AddOpenApi();

builder.Services.AddSharedApplicationConfiguration([Events.Application.AssemblyReference.Assembly]);
// string? connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
const string connectionString = "Server=localhost; Port=5432; Username=postgres; Password=password; Database=Tixora;";
builder.Services.AddInfrastructureSharedConfiguration(connectionString);

builder.Services.AddEventModule(connectionString);

builder.Configuration.AddModulesConfiguration(["events"]);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.AddEventEndpoints();

app.Run();
