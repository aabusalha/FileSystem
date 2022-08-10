using System;

namespace FileSystem.Api.Resources
{
    public class AttachmentRes
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string DownloadURL { get; set; }
    }
}
