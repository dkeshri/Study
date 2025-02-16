using Dkeshri.WebApi.Controllers.Base;
using Dkeshri.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Dkeshri.WebApi.Controllers
{
    public class WeatherForecastController : WebApiControllerBase
    {
        ISingleton singleton;
        IScope scope;
        ITransient transient;
        LoggerService loggerService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ISingleton singleton,IScope scope,ITransient transient,
            LoggerService loggerService)
        {
            _logger = logger;
            this.singleton = singleton;
            this.transient = transient;
            this.scope = scope;
            this.loggerService = loggerService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {


            Console.WriteLine("Weather Controller");
            Console.WriteLine("Singletone "+singleton.Id);
            Console.WriteLine("Scope "+scope.Id);
            Console.WriteLine("Transient "+transient.Id);

            loggerService.PrintGuid();


            using (LogContext.PushProperty("UserName", "user"))
            using (LogContext.PushProperty("UserId", 122))
            {
                _logger.LogInformation("Weather controller Get request");
            }
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            
        }
    }
}
