using IOTConcept.WebApi.Extensions;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRbbitMq();  
builder.Services.AddInfluxDb(configuration);
builder.Services.AddMediatR();
builder.Services.AddMetricServer(options =>
{
    //options.Port = 1234;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.Run();
