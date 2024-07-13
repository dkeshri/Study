using Dkeshri.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Dkeshri.WebApi.Controllers.SystemDesign.LowLevel.DesignPatterns
{
    public class BehavioralController : DesignPatternControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("TemplateMethod")]
        public IEnumerable<WeatherForecast> Get()
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }

        [HttpGet("TemplateMethodTest")]
        public IEnumerable<WeatherForecast> GetTest()
        {

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
