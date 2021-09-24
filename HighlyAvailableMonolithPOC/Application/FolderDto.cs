using System;
using System.Collections.Generic;

namespace HighlyAvailableMonolithPOC.Application
{
    public class FolderDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string DisplayName { get; set; }
        public List<FileDto> Files { get; set; } = new List<FileDto>();
        public List<FolderDto> SubFolders { get; } = new List<FolderDto>();

        public class FileDto
        {
            public Guid Id { get; set; }
            public string FileName { get; set; }
        }
    }
}
