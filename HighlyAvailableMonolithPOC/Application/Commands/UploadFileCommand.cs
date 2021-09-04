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
        private readonly ApplicationDbContext context;

        public UploadFileCommandHandler(ApplicationDbContext context)
        {
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

                    await context.SaveChangesAsync();
                    await ProcessAndSave(request, destinationPath, destinationFileName);
                }
            }

            return Unit.Value;
        }

        private static async Task ProcessAndSave(UploadFileCommand request, string destinationPath, string destinationFileName)
        {
            await Task.Delay(3000);  // Simulate some long running processing...

            Directory.CreateDirectory(destinationPath);
            using (var stream = System.IO.File.Create(destinationFileName))
            {
                await request.Content.CopyToAsync(stream);
            }
        }
    }
}
