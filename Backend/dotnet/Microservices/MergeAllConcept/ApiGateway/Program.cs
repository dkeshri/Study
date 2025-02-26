using ApiGateway.Extensions;
using ApiGateway.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOcelot();
builder.Services.AddApiAuthentication(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<PrometheusMetricsMiddleware>();
// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpMetrics();
app.UseOcelot().Wait();
app.MapMetrics();
app.MapControllers();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
