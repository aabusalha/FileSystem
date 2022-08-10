using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace FileSystem.Api.Resources
{
    public class EditFileRes
    {
        [Required]
        public int Id { get; set; }

      
        [Required]
        public string English { get; set; }

        public List<AttachmentRes> Attachments
        {
            set;
            get;
        }

    }
}
