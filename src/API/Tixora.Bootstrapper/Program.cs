using Events.Api;
using Scalar.AspNetCore;
using Serilog;
using Tickets.Api;
using Tixora.Bootstrapper.Extensions;
using Tixora.Bootstrapper.Middleware;
using Tixora.Shared.Application;
using Tixora.Shared.Infrastructure;
using Users.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddSharedApplicationConfiguration([
    Events.Application.AssemblyReference.Assembly, 
    Users.Application.AssemblyReference.Assembly,
    Tickets.Application.AssemblyReference.Assembly]);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException();
string redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException();
builder.Services.AddInfrastructureSharedConfiguration(databaseConnectionString, redisConnectionString,
[Tickets.Infrastructure.TicketsModule.ConfigureConsumers]);

builder.Services.AddEventModule(databaseConnectionString);
builder.Services.AddUsersModule(databaseConnectionString);
builder.Services.AddTicketsModule(databaseConnectionString);

builder.Configuration.AddModulesConfiguration(["events", "users", "tickets"]);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapEndpoints(app);

app.UseExceptionHandler();

app.Run();
