using Dapper;
using HighlyAvailableMonolithPOC.Infrastructure;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly FileStore fileStore;
        private readonly SqlConnectionFactory factory;
        private readonly ApplicationDbContext context;

        public DeleteFolderCommandHandler(
            FileStore fileStore,
            SqlConnectionFactory factory,
            ApplicationDbContext context)
        {
            this.fileStore = fileStore;
            this.factory = factory;
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await context.Folders.FirstOrDefaultAsync(x => x.Id == request.FolderId);
            if (folder != null)
            {
                var paths = await GetPaths(request.FolderId);

                await fileStore.Remove(paths);
                context.Folders.Remove(folder);

                //await context.SaveChangesAsync();
            }

            return Unit.Value;
        }

        private async Task<List<string>> GetPaths(Guid targetFolderId)
        {
            FolderDto rootFolder;

            using (var connection = factory.GetOpenConnection())
            {
                string sql = @"SELECT [Id], [ParentId], [DisplayName] 
                               FROM [dbo].[Folders];
                              
                               SELECT [Id], [FileName], [FolderId]
                               FROM [dbo].[File]";

                using (var multi = await connection.QueryMultipleAsync(sql))
                {

                    var folderInfos = (await multi.ReadAsync<FolderInfo>()).AsList();
                    var fileInfos = (await multi.ReadAsync<FileInfo>()).AsList();

                    rootFolder = TreeFolderBuilder.Build(folderInfos, fileInfos, targetFolderId);
                }
            }

            if (rootFolder == null)
                return new List<string>();

            var flattenedFolders = Flatten(rootFolder);
            var filenames = flattenedFolders.SelectMany(ff => ff.Files.Select(f => f.FileName));

            return filenames.ToList();
        }

        public IEnumerable<FolderDto> Flatten(FolderDto root)
        {
            var folders = new Stack<FolderDto>();
            folders.Push(root);

            while (folders.Count > 0)
            {
                var folder = folders.Pop();
                yield return folder;

                for (int i = folder.SubFolders.Count - 1; i >= 0; i--)
                {
                    folders.Push(folder.SubFolders[i]);
                }
            }
        }
    }
}
