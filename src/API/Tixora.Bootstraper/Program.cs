using Events.Api;
using Scalar.AspNetCore;
using Tixora.Shared.Application;
using Tixora.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSharedApplicationConfiguration([Events.Application.AssemblyReference.Assembly]);
// string? connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
const string connectionString = "Server=localhost; Port=5432; Username=postgres; Password=password; Database=Tixora;";
builder.Services.AddInfrastructureSharedConfiguration(connectionString);

builder.Services.AddEventModule(connectionString);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.AddEventEndpoints();

app.Run();
