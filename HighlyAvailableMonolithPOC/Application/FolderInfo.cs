using System;

namespace HighlyAvailableMonolithPOC.Application
{
    internal class FolderInfo
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string DisplayName { get; set; }
    }

    internal class FileInfo
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public string FileName { get; set; }
    }
}
