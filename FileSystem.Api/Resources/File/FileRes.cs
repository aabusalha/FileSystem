using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSystem.Api.Resources
{
    public class FileRes
    {
        public int Id { get; set; }
        public string Level { get; set; }
       
        public string English { get; set; }
       
       
        public IEnumerable<FileRes> Childs { get; set; }

        public IEnumerable<AttachmentRes> Attachments { get; set; }
    }
}
