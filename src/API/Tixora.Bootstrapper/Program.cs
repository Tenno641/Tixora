using Events.Api;
using Scalar.AspNetCore;
using Serilog;
using Tixora.Bootstrapper.Extensions;
using Tixora.Bootstrapper.Middleware;
using Tixora.Shared.Application;
using Tixora.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddSharedApplicationConfiguration([Events.Application.AssemblyReference.Assembly]);
// string? connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
const string connectionString = "Server=localhost; Port=5432; Username=postgres; Password=password; Database=Tixora;";
builder.Services.AddInfrastructureSharedConfiguration(connectionString);

builder.Services.AddEventModule(connectionString);

builder.Configuration.AddModulesConfiguration(["events"]);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapEndpoints(app);

app.UseExceptionHandler();

app.Run();
