using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dkeshri.AzureFunctions.Functions
{
    public class HttpTriggerFunction
    {
        private readonly ILogger<HttpTriggerFunction> _logger;
        private DemoDependency _demoDependency;
        public HttpTriggerFunction(ILogger<HttpTriggerFunction> logger, DemoDependency demoDependency)
        {
            _logger = logger;
            _demoDependency = demoDependency;
        }

        [Function("HttpTriggerFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _demoDependency.LogDemoDependency();
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
