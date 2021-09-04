using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace HighlyAvailableMonolithPOC.Infrastructure.Persistence
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public ICollection<File> Files { get; set; } = new List<File>();

        public Folder Parent { get; set; }
        public Guid? ParentId { get; set; }
        public ICollection<Folder> SubFolders { get; } = new List<Folder>();
    }

    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable("Folder");
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Files).WithOne(x => x.Folder);
            builder.HasOne(x => x.Parent).WithMany(x => x.SubFolders).HasForeignKey(x => x.ParentId);
        }
    }
}
