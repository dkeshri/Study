using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Dkeshri.AzureFunctions.Functions
{
    public class HttpTriggerFunction
    {
        private readonly ILogger _logger;
        private DemoDependency demoDependency;
        public HttpTriggerFunction(ILoggerFactory loggerFactory, DemoDependency demoDependency)
        {
            _logger = loggerFactory.CreateLogger<HttpTriggerFunction>();
            this.demoDependency = demoDependency;
        }

        [Function("HttpTriggerFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            demoDependency.LogDemoDependency();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
