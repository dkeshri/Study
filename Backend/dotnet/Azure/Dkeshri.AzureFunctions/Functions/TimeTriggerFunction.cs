using System;
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
        public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
