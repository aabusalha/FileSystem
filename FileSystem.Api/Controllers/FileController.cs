using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using AutoMapper;
using FileSystem.Api.Resources;
using FileSystem.Core.Models;
using FileSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class FileController : ControllerBase
    {
        private readonly FileService _FileService;
        private readonly IMapper _mapper;
        private readonly StorageService _storageService;
        
        public FileController(FileService FileService, StorageService storageService, IMapper mapper)
        {
            _mapper = mapper;
            _storageService = storageService;
            _FileService = FileService;
        }


        [HttpGet("Tree/AllFile")]
        public async Task<ActionResult<IEnumerable<FileRes>>> GetAllFileTree(int? parentId)
        {
            IEnumerable<File> files = await _FileService.GetAllFileTree(parentId);

            return Ok(_mapper.Map<IEnumerable<File>, IEnumerable<FileRes>>(files));
        }
            
        [HttpGet("{Id}")]
        public async Task<ActionResult<FileRes>> GetFile(int Id)
        {
            File file = await _FileService.GetFile(Id);
            return Ok(_mapper.Map<File, FileRes>(file));
        }

        [HttpPost]
        public async Task<ActionResult<FileRes>> AddFile(SavefileRes fileResource, int? parentId)
        {

            var file = _mapper.Map<SavefileRes, File>(fileResource);

            var result = await _FileService.CreateFile(file, parentId);

            if (result.Id > 0)
            {
                File createdFile = await _FileService.GetFile(result.Id);
              
                return Created(string.Empty, _mapper.Map<File, FileRes>(createdFile));
            }

            return Problem("Error", null, 500);
        }

        [HttpPut]
        public async Task<ActionResult<FileRes>> EditFile(EditFileRes fileResource)
        {
            try
            {
            

                var fileToBeUpdated = await _FileService.GetFile(fileResource.Id);


                if (fileToBeUpdated == null)
                    return NotFound();

                var file = _mapper.Map<EditFileRes, File>(fileResource);


                await _FileService.UpdateFile(fileToBeUpdated,file);

                var updatedFile = await _FileService.GetFile(fileResource.Id);
                var updatedFileRes = _mapper.Map<File, FileRes>(updatedFile);
             
                return Created(string.Empty, updatedFileRes);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, 500);
            }
        }
            
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteFile(int Id)
        {
            try
            {
                await _FileService.DeleteFile(Id);
                return Ok(string.Empty);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, 500);
            }
        }


        [HttpPost("FolderId"), DisableRequestSizeLimit]
        public async Task<ActionResult<string>> Upload(IFormFile file, int FolderId)
        {
            try
            {
                File folder = await _FileService.GetFile(FolderId);
                
                
                if (folder != null)
                {
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName;
                        byte[] bytes = null;
                        using (var fileStream = file.OpenReadStream())
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            bytes = memoryStream.ToArray();
                        }
                        var attachment = new Attachment()
                        {
                            Name = fileName,
                            Data = bytes,
                            Extension = System.IO.Path.GetExtension(fileName),
                        };
                        var attachmentResult = await _storageService.UploadFile(attachment);
                        var attachmentRes = _mapper.Map<Attachment, AttachmentRes>(attachmentResult);
                        if (attachmentResult !=null)
                        {
                            
                            await _FileService.UploadFile(folder, attachmentResult);
                        }
                       
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound("Folder not found");
                }

               
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}