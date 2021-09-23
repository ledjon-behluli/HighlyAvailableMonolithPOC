using HighlyAvailableMonolithPOC.Infrastructure;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly FileStore fileStore;
        private readonly ApplicationDbContext context;

        public DeleteFolderCommandHandler(
            FileStore fileStore,
            ApplicationDbContext context)
        {
            this.fileStore = fileStore;
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await context.Folders
                .Include(f => f.Files)
                .Include(f => f.SubFolders)
                    .ThenInclude(f => f.Files)
                .FirstOrDefaultAsync(x => x.Id == request.FolderId);
            
            if (folder != null)
            {
                var filesNames = folder.Files.Select(f => f.FileName).ToList();

                await fileStore.Remove(filesNames);
                context.Folders.Remove(folder);

                //await context.SaveChangesAsync();
            }

            return Unit.Value;
        }
    }
}
