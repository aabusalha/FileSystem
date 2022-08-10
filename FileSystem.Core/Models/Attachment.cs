using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.Core.Models
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }

        public Attachment() {
            Id = Guid.NewGuid();
        }
    }
}
