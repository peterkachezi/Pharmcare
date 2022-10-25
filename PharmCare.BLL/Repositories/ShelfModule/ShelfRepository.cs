
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using AutoMapper;
using PharmCare.DAL.Models;
using PharmCare.DTO.ShelfModule;

namespace PharmCare.BLL.Repositories.ShelfModule
{
    public class ShelfRepository : IShelfRepository
    {
        private ApplicationDbContext context;

        private readonly IMapper mapper;
        public ShelfRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<ShelfDTO> Create(ShelfDTO shelfDTO)
        {
            try
            {
                shelfDTO.Id = Guid.NewGuid();

                shelfDTO.CreateDate = DateTime.Now;

                var shelf = mapper.Map<Shelf>(shelfDTO);

                context.Shelves.Add(shelf);

                await context.SaveChangesAsync();

                return shelfDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var s = await context.Shelves.FindAsync(Id);

                if (s != null)
                {
                    context.Shelves.Remove(s);

                    await context.SaveChangesAsync();

                    return true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<List<ShelfDTO>> GetAll()
        {
            try
            {
                var shelves = (from s in context.Shelves

                               join u in context.AppUsers on s.CreatedBy equals u.Id

                               select new ShelfDTO
                               {
                                   Id = s.Id,

                                   Name = s.Name,

                                   CreateDate = s.CreateDate,

                                   CreatedBy = s.CreatedBy,

                                   CreatedByName = u.FirstName + " " + u.LastName,

                               }).ToListAsync();

                return await shelves;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ShelfDTO> GetById(Guid Id)
        {
            try
            {
                var shelf = (from s in context.Shelves

                             join u in context.AppUsers on s.CreatedBy equals u.Id

                             where s.Id == Id

                             select new ShelfDTO
                             {
                                 Id = s.Id,

                                 Name = s.Name,

                                 CreateDate = s.CreateDate,

                                 CreatedBy = s.CreatedBy,

                                 CreatedByName = u.FirstName + " " + u.LastName,

                             }).FirstOrDefaultAsync();

                return await shelf;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ShelfDTO> Update(ShelfDTO shelfDTO)
        {
            try
            {
                var location = await context.Shelves.FindAsync(shelfDTO.Id);

                if (location != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        location.Name = shelfDTO.Name;

                        location.UpdatedBy = shelfDTO.UpdatedBy;

                        location.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return shelfDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(ShelfDTO shelfDTO)
        {
            try
            {
                bool result = await context.Shelves.AnyAsync(p => p.Name == shelfDTO.Name);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
