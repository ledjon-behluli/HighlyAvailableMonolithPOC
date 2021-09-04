using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HighlyAvailableMonolithPOC.Infrastructure.Persistence
{
    public class File
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }

        public Guid FolderId { get; set; }
        public Folder Folder { get; set; }
    }

    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("File");
            builder.HasKey(x => x.Id);
        }
    }
}
