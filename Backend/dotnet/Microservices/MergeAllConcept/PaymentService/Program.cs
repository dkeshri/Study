using PaymentService.Extensions;
using PaymentService.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();
builder.Services.AddMassTransit(builder.Configuration);
var app = builder.Build();
app.MigrateDatabase();
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
