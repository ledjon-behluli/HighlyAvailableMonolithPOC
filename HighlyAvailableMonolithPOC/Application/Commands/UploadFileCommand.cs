using HighlyAvailableMonolithPOC.Application;
using HighlyAvailableMonolithPOC.Infrastructure;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Folders.Commands
{
    public class UploadFileCommand : IRequest<Unit>
    {
        public Guid FolderId { get; set; }
        public string FileName { get; set; }
        public Stream Content { get; set; }
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Unit>
    {
        private readonly FileStore fileStore;
        private readonly ApplicationDbContext context;

        public UploadFileCommandHandler(
            FileStore fileStore,
            ApplicationDbContext context)
        {
            this.fileStore = fileStore;
            this.context = context;
        }

        public async Task<Unit> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if (request.Content.Length > 0)
            {
                string ext = Path.GetExtension(request.FileName);
                string displayName = Path.GetFileNameWithoutExtension(request.FileName);
                string destinationPath = $"{AppDomain.CurrentDomain.BaseDirectory}filestore";
                string destinationFileName = $"{destinationPath}\\{displayName}-{Guid.NewGuid()}{ext}";

                var folder = await context.Folders.FirstOrDefaultAsync(x => x.Id == request.FolderId);

                if (folder != null)
                {
                    folder.Files.Add(new Infrastructure.Persistence.File()
                    {
                        DisplayName = displayName,
                        FileName = destinationFileName,
                        FolderId = request.FolderId
                    });

                    await fileStore.Add(request.Content, destinationPath, destinationFileName);
                    await context.SaveChangesAsync();
                }
            }

            return Unit.Value;
        }
    }
}
