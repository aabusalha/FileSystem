using System.Threading.Tasks;
using FileSystem.Core;
using FileSystem.Core.Models;
using FileSystem.Core.Repositories;
using FileSystem.Data.Repositories;

namespace FileSystem.Data
{
        public class UnitOfWork
    {
        private readonly FileSystemDBContext _context;
      
        private StorageRepository _storageRepository;
       

        private FileRepository _fileRepository;

     
        public UnitOfWork(FileSystemDBContext context)
        {
            this._context = context;
        }

      
        public StorageRepository Storage => _storageRepository = _storageRepository ?? new StorageRepository(_context);
  
        public FileRepository File => _fileRepository = _fileRepository ?? new FileRepository(_context);
       
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}