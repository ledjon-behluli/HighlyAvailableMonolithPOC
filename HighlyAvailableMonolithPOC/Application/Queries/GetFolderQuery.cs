using Dapper;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Application.Queries
{
    public class GetFolderQuery : IRequest<FolderDto>
    {
        public Guid FolderId { get; set; }
    }

    public class GetFolderQueryHandler : IRequestHandler<GetFolderQuery, FolderDto>
    {
        private readonly SqlConnectionFactory factory;

        public GetFolderQueryHandler(SqlConnectionFactory factory)
        {
            this.factory = factory;
        }

        public async Task<FolderDto> Handle(GetFolderQuery request, CancellationToken cancellationToken)
        {
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

                    return ConstructResponse(folderInfos, fileInfos, request.FolderId);
                }
            }
        }

        private FolderDto ConstructResponse(List<FolderInfo> folderInfos, List<FileInfo> fileInfos, Guid targetFolderId)
        {
            List<FolderDto> folderDtos = folderInfos
                  .Select(i => new FolderDto()
                  {
                      Id = i.Id,
                      DisplayName = i.DisplayName,
                      ParentId = i.ParentId
                  }).ToList();

            var root = folderDtos.FirstOrDefault(i => i.Id == targetFolderId);
            var result = BuildTree(root, folderDtos, fileInfos);

            return result;
        }

        private FolderDto BuildTree(FolderDto root, List<FolderDto> folders, List<FileInfo> fileInfos)
        {
            AttachFiles(root, fileInfos);

            if (folders.Count == 0)
                return root;

            var subFolders = folders.Where(n => n.ParentId == root.Id).ToList();

            foreach (var folder in subFolders)
            {
                AttachFiles(folder, fileInfos);
            }

            root.SubFolders.AddRange(subFolders);

            foreach (var folder in root.SubFolders)
            {
                folders.Remove(folder);
            }

            for (int i = 0; i < subFolders.Count; i++)
            {
                subFolders[i] = BuildTree(subFolders[i], folders, fileInfos);
                if (folders.Count == 0)
                {
                    break;
                }
            }

            return root;
        }

        private void AttachFiles(FolderDto folder, List<FileInfo> fileInfos)
        {
            folder.Files = fileInfos
                .Where(i => i.FolderId == folder.Id)
                .Select(i => new FolderDto.FileDto()
                {
                    Id = i.Id,
                    FileName = i.FileName
                }).ToList();
        }


        private class FolderInfo
        {
            public Guid Id { get; set; }
            public Guid? ParentId { get; set; }
            public string DisplayName { get; set; }
        }

        private class FileInfo
        {
            public Guid Id { get; set; }
            public Guid FolderId { get; set; }
            public string FileName { get; set; }
        }
    }
}
