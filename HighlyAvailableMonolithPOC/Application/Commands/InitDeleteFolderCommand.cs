using DotNetCore.CAP;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Application.Commands
{
    public class InitDeleteFolderCommand : IRequest<Unit>
    {
        public Guid FolderId { get; set; }
    }

    public class InitDeleteFolderCommandHandler : IRequestHandler<InitDeleteFolderCommand, Unit>
    {
        private readonly ICapPublisher capPublisher;

        public InitDeleteFolderCommandHandler(ICapPublisher capPublisher)
        {
            this.capPublisher = capPublisher;
        }

        public async Task<Unit> Handle(InitDeleteFolderCommand request, CancellationToken cancellationToken)
        {
            await capPublisher.PublishAsync("delete.folder", request.FolderId);
            return Unit.Value;
        }
    }
}