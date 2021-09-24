using Dapper;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using System;
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

                    return TreeFolderBuilder.Build(folderInfos, fileInfos, request.FolderId);
                }
            }
        }
    }
}
