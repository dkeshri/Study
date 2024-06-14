using Serilog;
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() // Set the minimum log level
            .WriteTo.Console() // Configure a sink, e.g., Console
            .CreateLogger();
try
{
    Log.Information("Init Main");
    var builder = WebApplication.CreateBuilder(args);

    // Add logging
    builder.Host.UseSerilog();


    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    

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

