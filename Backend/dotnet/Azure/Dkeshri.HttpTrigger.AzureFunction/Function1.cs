using System;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Dkeshri.HttpTrigger.AzureFunction
{
    public class Function1
    {
        private readonly ILogger _logger;
        private TestClass _testClass;

        public Function1(ILoggerFactory loggerFactory,TestClass testClass)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _testClass = testClass;
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            Console.WriteLine("Deepsk Guid "+ _testClass.ID);

            return response;
        }
    }
}
