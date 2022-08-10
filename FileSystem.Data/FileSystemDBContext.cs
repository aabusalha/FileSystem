
using FileSystem.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileSystem.Data
{
    public class FileSystemDBContext : DbContext
    {
        
        public DbSet<Attachment> Attachments { get; set; }
      
        public DbSet<File> Files { get; set; }
       
        public FileSystemDBContext(DbContextOptions<FileSystemDBContext> options)
            : base(options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(conf =>
            {
                conf.UseHierarchyId();
            });

        }
       
    }
}