using Dkeshri.WebApi.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add logging
    builder.Host.AddSerilog(builder.Configuration);

    Log.Information("Serilog configured");
    // Add services to the container.
    builder.Services.AddServices();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddLogLevelTest();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Starting web host");
    app.Run();
    
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

