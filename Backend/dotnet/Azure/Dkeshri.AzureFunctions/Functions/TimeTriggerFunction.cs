using System;
using System.Text.Json;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dkeshri.AzureFunctions.Functions
{
    public class TimeTriggerFunction
    {
        private readonly ILogger _logger;

        public TimeTriggerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimeTriggerFunction>();
        }

        [Function("TimeTriggerFunction")]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            var timestampUtc = DateTime.UtcNow;
            _logger.LogInformation($"C# Timer trigger function executed at: {timestampUtc}");
            // Ensure container exists (one-time setup)
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage")!;
            // Define container and blob file name
            string containerName = "my-container";
            string blobFileName = "data.json";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();


            // Get reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(blobFileName);
            // Define new JSON content
            var newData = new
            {
                timestampUtc = timestampUtc,
                message = "Updated JSON content"
            };

            // Convert object to JSON string
            string jsonContent = JsonSerializer.Serialize(newData, new JsonSerializerOptions { WriteIndented = true });

            // Upload JSON content as a blob (overwrite existing file)
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            _logger.LogInformation($"Updated {blobFileName} in {containerName} successfully.");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
