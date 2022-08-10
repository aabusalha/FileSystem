using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using FileSystem.Core;
using FileSystem.Core.Models;
using FileSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace FileSystem.Services
{
    public class FileService
    {

        private readonly UnitOfWork _unitOfWork;

        public FileService(UnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }
        public async Task<File> CreateFile(File file, int? ParentId)
        {
            HierarchyId parentItemLevel;
            File lastItemInCurrentLevel;

            if (!ParentId.HasValue)
            {
                parentItemLevel = HierarchyId.GetRoot();
            }
            else
            {
                var parent = _unitOfWork.File.GetByIdAsync(ParentId.Value).Result;
                if (parent == null)
                    throw new Exception("ParentId does not exist");

                parentItemLevel = parent.Level;
            }

            lastItemInCurrentLevel = _unitOfWork.File.GetLastFileAsync(parentItemLevel).Result;

            var child1Level = lastItemInCurrentLevel != null ? lastItemInCurrentLevel.Level : null;

            var newLevel = parentItemLevel.GetDescendant(child1Level, null);
            file.Level = newLevel;
          
            await _unitOfWork.File.AddAsync(file);

            await _unitOfWork.CommitAsync();

            return file;
        }

        public async Task<bool> DeleteFile(int id)
        {
            var File =  _unitOfWork.File.GetByIdAsync(id).Result;
            _unitOfWork.File.Remove(File);
            await _unitOfWork.CommitAsync();
            return true;
        }

       



        public async Task<IEnumerable<File>> GetAllFileTree(int? ParentId)
        {
            File parent;
            IEnumerable<File> files;
            HierarchyId parentLevel = null;
            if (ParentId.HasValue)
            {
                parent = _unitOfWork.File.GetByIdAsync(ParentId.Value).Result;
                parentLevel = parent.Level;
                files = await _unitOfWork.File.GetAllFileTreeAsync(parent.Level);
            }
            else
            {
                files = await _unitOfWork.File.GetAllFileTreeAsync(null);
            }



            foreach (var file in files)
            {
                file.Childs = files.Where(e => e.Level.GetAncestor(1) == file.Level).ToList();
               

            }

            if (parentLevel != null)
                return files.Where(e => e.Level.GetAncestor(1) == parentLevel).ToList();
            return files.Where(e => e.Level.GetAncestor(1) == HierarchyId.GetRoot()).ToList();
        }





     
        public async Task<File> GetFile(int Id)
        {
            var file = await _unitOfWork.File.GetByIdAsync(Id);
            return file;
        }

        public async Task<bool> UpdateFile(File fileToBeUpdated, File file)
        {
            
           
            fileToBeUpdated.English = file.English;
            fileToBeUpdated.Attachments = file.Attachments;
          
                _unitOfWork.File.UpdateFile(fileToBeUpdated);

            await _unitOfWork.CommitAsync();
            return true;
        }
        public async Task<bool> UploadFile(File fileToBeUpdated, Attachment att)
        {


            _unitOfWork.File.UploadFile(fileToBeUpdated, att);

            await _unitOfWork.CommitAsync();
            return true;
        }



    }
}
