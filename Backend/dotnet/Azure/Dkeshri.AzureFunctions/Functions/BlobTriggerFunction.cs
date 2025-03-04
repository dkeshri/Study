using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dkeshri.AzureFunctions.Functions
{
    public class BlobTriggerFunction
    {
        private readonly ILogger<BlobTriggerFunction> _logger;

        public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobTriggerFunction))]
        public async Task Run([BlobTrigger("my-container/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
        {
            // Ensure container exists (one-time setup)
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage")!;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("my-container");
            await containerClient.CreateIfNotExistsAsync();

            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
