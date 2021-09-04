using MediatR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Diagnostics;
using System;

namespace HighlyAvailableMonolithPOC.Application.Pipelines
{
    public class MetricsPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<MetricsPipeline<TRequest, TResponse>> logger;

        public MetricsPipeline(ILogger<MetricsPipeline<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation("Starting request {RequestName}", typeof(TRequest).Name);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = await next();

            stopwatch.Stop();

            logger.LogInformation("Request {RequestName} finished and took {Time}", typeof(TRequest).Name, TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds));
            return response;
        }
    }
}
