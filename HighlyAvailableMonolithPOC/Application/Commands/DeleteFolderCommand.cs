using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Application.Commands
{
    public class DeleteFolderCommand : IRequest<Unit>
    {
        public Guid FolderId { get; set; }
    }

    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand, Unit>
    {
        private readonly ApplicationDbContext context;

        public DeleteFolderCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await context.Folders.FirstOrDefaultAsync(x => x.Id == request.FolderId);
            if (folder != null)
            {
                var filesNames = folder.Files.Select(f => f.FileName).ToList();

                // TODO: Get all files within subfolders...

                context.Folders.Remove(folder);

                await context.SaveChangesAsync();
                await DeleteAllFilesFromStore(filesNames);
            }

            return Unit.Value;
        }

        private async Task DeleteAllFilesFromStore(IEnumerable<string> fileNames)
        {
            throw new NotImplementedException();
        }
    }
}
