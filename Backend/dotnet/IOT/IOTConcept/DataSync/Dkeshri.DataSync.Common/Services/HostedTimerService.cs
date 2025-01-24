using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Dkeshri.DataSync.Common.Services
{
    public abstract class HostedTimerService : IHostedService, IDisposable
    {
        private TimeSpan Interval { get; set; }
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isDisposing;
        protected HostedTimerService(TimeSpan timeSpan)
        {
            Interval = timeSpan;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        protected abstract Task OperationToPerform(CancellationToken cancellationToken);
        protected virtual Task OnStartup(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Hosted timer service");
            try
            {
                await OnStartup(cancellationToken);
                await Task.Delay(1000, cancellationToken);
                new Thread(async () => {
                    using var periodicTimer = new PeriodicTimer(Interval);
                    while (!_cancellationTokenSource.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync(cancellationToken))
                    {
                        using var childCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
                        var stopWatch = Stopwatch.StartNew();
                        try
                        {
                            childCancellationTokenSource.CancelAfter((int)TimeSpan.FromMinutes(5).TotalMilliseconds);
                            await OperationToPerform(childCancellationTokenSource.Token).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException operationCanceledException) when (childCancellationTokenSource.IsCancellationRequested
                        && !_cancellationTokenSource.IsCancellationRequested)
                        {

                            Console.WriteLine("Hosted service timeout while execution operation");
                            Console.WriteLine(operationCanceledException.Message);
                        }
                        catch (TaskCanceledException)
                        {
                            // this exception is thrown on shutdown.
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                    }
                })
                { IsBackground = true }.Start();
                Console.WriteLine("Hosted timer service has been started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start {ex}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hosted timer service is stopping...");
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            Console.WriteLine("Hosted timer service is stopped");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposing)
            {
                return;
            }
            if (disposing)
            {
                _cancellationTokenSource.Dispose();
            }
            _isDisposing = true;
        }
    }
}
