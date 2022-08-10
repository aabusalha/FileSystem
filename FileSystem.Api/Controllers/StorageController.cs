using AutoMapper;
using FileSystem.Api.Resources;
using FileSystem.Core.Models;
using FileSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileSystem.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class StorageController : ControllerBase
    {
        private readonly StorageService _storageService;
        private readonly IMapper _mapper;
        public StorageController(StorageService storageService,
            IMapper mapper
            )
        {
            _storageService = storageService;
            _mapper = mapper;
        }


        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileName = file.FileName;
                    byte[] bytes = null;
                    using (var fileStream = file.OpenReadStream())
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                    var attachment = new Attachment()
                    {
                        Name = fileName,
                        Data = bytes,
                        Extension = Path.GetExtension(fileName),
                    };
                    var attachmentResult = await _storageService.UploadFile(attachment);
                    var attachmentRes = _mapper.Map<Attachment, AttachmentRes>(attachmentResult);
                    return Ok(attachmentRes);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

      


        [HttpGet("{id}")]
     
        public IActionResult Download(string id)
        {
            var attachment = _storageService.DownloadFile(id);
            HttpContext.Request.Headers.Remove("If-Modified-Since");
            HttpContext.Request.Headers.Remove("If-None-Match");
            HttpContext.Request.Headers.Remove("Content-Length");
            var stream = new MemoryStream(attachment.Data);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, attachment.MimeType, attachment.Name);
        }


      
    }
}
