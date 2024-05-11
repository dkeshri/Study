using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication
{
    public class MyConsoleService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("My console Service is starting.");

            // Your application logic goes here
            await Task.Delay(1000);

            Console.WriteLine("My console Service has started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("My Console Service is stopping.");

            // Your clean-up logic goes here
            await Task.Delay(1000);

            Console.WriteLine("My Console Service has stopped.");
        }

        public void Dispose()
        {
            _stoppingCts?.Cancel();
            _stoppingCts?.Dispose();
        }
    }
}
