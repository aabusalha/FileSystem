using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;



namespace FileSystem.Core.Models
{
    public class File 
    {
        public File()
        {
            Childs = new List<File>();
        }

        [Key]
        public int Id { get; set; }
       public HierarchyId Level { get; set; }
      
        public string English { get; set; }
       

        [NotMapped]
        public IEnumerable<File> Childs { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
