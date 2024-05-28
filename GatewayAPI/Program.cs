using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

Task<IApplicationBuilder> task = app.UseOcelot();

app.Run();
