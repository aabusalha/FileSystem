using FileSystem.Core;
using FileSystem.Core.Models;
using FileSystem.Data;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.Services
{
    public class StorageService
    {
        private readonly UnitOfWork _unitOfWork;
        public StorageService(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }



        public Attachment DownloadFile(string id)
        {
            var file = _unitOfWork.Storage.DownloadFile(new Guid(id));
            return file;
        }


        public async Task<Attachment> UploadFile(Attachment attachment)
        {
            attachment = addMetaData(attachment);
            attachment =  await _unitOfWork.Storage.UploadFile(attachment);
            await _unitOfWork.CommitAsync();
            return attachment;
        }

     

        private Attachment addMetaData(Attachment attachment)
        {
            var provider = new FileExtensionContentTypeProvider();
            var contentType = provider.TryGetContentType(attachment.Name, out var type)
                ? type
                : "application/octet-stream";
            attachment.MimeType = contentType;
            return attachment;
        }
    }
}
