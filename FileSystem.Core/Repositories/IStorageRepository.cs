using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystem.Core.Models;

namespace FileSystem.Core.Repositories
{
    public interface IStorageRepository : IRepository<Attachment>
    {
        public Task<Attachment> UploadFile(Attachment attachment);
        public Attachment DownloadFile(Guid id);
      
    }
}
