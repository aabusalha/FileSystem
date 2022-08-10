using FileSystem.Core.Models;
using FileSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.Data.Repositories
{
    public class FileRepository : Repository<File>
    {
        public FileRepository(FileSystemDBContext context)
        : base(context)
        { }

        public async Task<File> GetLastFileAsync(HierarchyId ParentIdLevel)
        {
            return await Context.Files
                  .AsNoTracking()
                  .Where(x => x.Level.GetAncestor(1) == ParentIdLevel)
                  .OrderByDescending(x => x.Level)
                               .FirstOrDefaultAsync();
        }
    
       
        public void UpdateFile(File file)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (var item in file.Attachments)
            {
                if (item != null)
                {
                    Attachment tmp = new Attachment();
                    tmp = Context.Attachments.Where(a => a.Id == item.Id).FirstOrDefault();
                    if (tmp != null)

                        attachments.Add( tmp);

                }
            }

            file.Attachments = attachments;
            Context.Update(file);
            //Context.SaveChanges();
        }
        public void UploadFile(File file, Attachment att)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (var item in file.Attachments)
            {
                if (item != null)
                {
                    Attachment tmp = new Attachment();
                    tmp = Context.Attachments.Where(a => a.Id == item.Id).FirstOrDefault();
                    if (tmp != null)

                        attachments.Add(tmp);

                }
            }
            attachments.Add(att);

            file.Attachments = attachments;
            Context.Update(file);
            //Context.SaveChanges();
        }




        public async override Task AddAsync(File file)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (var item in file.Attachments)
            {
                if (item != null)
                {
                    Attachment tmp = new Attachment();
                    tmp = Context.Attachments.Where(a => a.Id == item.Id).FirstOrDefault();
                    if (tmp != null)

                        attachments.Add(tmp);

                }
            }

            file.Attachments = attachments;
            Context.Attach(file);
            Context.Entry(file).Reload();
            await base.AddAsync(file);
        }

        public async Task<IEnumerable<File>> GetAllFileTreeAsync(HierarchyId ParentIdLevel)
        {
            
            IEnumerable<File> allFiles= await Context.Files.Include(a => a.Attachments)
                .Where(e => (ParentIdLevel != null && e.Level.IsDescendantOf(ParentIdLevel)) ||
                    (ParentIdLevel == null && e.Level.IsDescendantOf(HierarchyId.GetRoot())))
               
                .ToListAsync();

          

   
            return allFiles;

         
        }




       
        public async override ValueTask<File> GetByIdAsync(int id)
        {
            return await Context.Files.Where(e => e.Id == id).Include(a => a.Attachments)

                               .SingleOrDefaultAsync();

           
        }
    }
}