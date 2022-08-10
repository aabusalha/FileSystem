using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileSystem.Api.Resources
{
    public class SavefileRes
    {
       
        [Required]
        public string English { get; set; }
        public List<AttachmentRes> Attachments
        {
            set;
            get;
        }

    }
}
