using AuthService.Interfaces;
using AuthService.Middleware;
using AuthService.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();    
builder.Services.AddScoped<PrometheusMetricsMiddleware>();

var app = builder.Build();
app.UseMiddleware<PrometheusMetricsMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHttpMetrics();
app.MapControllers();
app.MapMetrics();
app.MapControllers();
app.Run();
