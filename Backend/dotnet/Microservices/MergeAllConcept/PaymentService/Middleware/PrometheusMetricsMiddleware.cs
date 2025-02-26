using Prometheus;

namespace PaymentService.Middleware
{
    public class PrometheusMetricsMiddleware : IMiddleware
    {

        private readonly Counter _requestCounter = Metrics.CreateCounter("http_requests_total", "Total number of requests");

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _requestCounter.Inc(); // Increment the counter for each request

            await next(context);  // Continue the request pipeline
        }
    }
}
