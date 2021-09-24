using System;
using System.Collections.Generic;
using System.Linq;

namespace HighlyAvailableMonolithPOC.Application
{
    internal static class TreeFolderBuilder
    {
        internal static FolderDto Build(List<FolderInfo> folderInfos, List<FileInfo> fileInfos, Guid targetFolderId)
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

        private static FolderDto BuildTree(FolderDto root, List<FolderDto> folders, List<FileInfo> fileInfos)
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

        private static void AttachFiles(FolderDto folder, List<FileInfo> fileInfos)
        {
            folder.Files = fileInfos
                .Where(i => i.FolderId == folder.Id)
                .Select(i => new FolderDto.FileDto()
                {
                    Id = i.Id,
                    FileName = i.FileName
                }).ToList();
        }
    }
}
