using DotNetCore.CAP;
using HighlyAvailableMonolithPOC.Application.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Infrastructure
{
    public class InitDeleteFolderHandler : ICapSubscribe
    {
        private readonly ISender sender;

        public InitDeleteFolderHandler(ISender sender)
        {
            this.sender = sender;
        }

        [CapSubscribe("delete.folder")]
        public async Task Handle(Guid folderId)
        {
            await sender.Send(new DeleteFolderCommand()
            {
                FolderId = folderId
            });
        }
    }
}
