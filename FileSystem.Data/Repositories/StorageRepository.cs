using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using FileSystem.Core.Repositories;
using FileSystem.Core.Models;

namespace FileSystem.Data.Repositories
{
    public class StorageRepository : Repository<Attachment>, IStorageRepository
    {
        public StorageRepository(FileSystemDBContext context)
           : base(context)
        { 
        }

        public Attachment DownloadFile(Guid id)
        {
            var file =  ((FileSystemDBContext)Context).Attachments.Find(id);
            return file;
        }

        public async Task<Attachment> UploadFile(Attachment attachment)
        {
            await ((FileSystemDBContext)Context).Attachments.AddAsync(attachment);
            return attachment;
        }

        
    }
}
