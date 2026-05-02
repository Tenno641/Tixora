using Events.Api;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEventDependencies();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.AddEventEndpoints();

app.Run();
