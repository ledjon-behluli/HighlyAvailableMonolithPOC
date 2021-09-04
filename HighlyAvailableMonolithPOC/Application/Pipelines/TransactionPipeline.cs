using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Application.Pipelines
{
    public class TransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ApplicationDbContext context;

        public TransactionPipeline(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = default;

            try
            {
                await context.Database.BeginTransactionAsync();

                response = await next();

                await context.Database.CommitTransactionAsync();

                return response;
            }
            catch
            {
                await context.Database.RollbackTransactionAsync();
            }

            return response;
        }
    }
}
