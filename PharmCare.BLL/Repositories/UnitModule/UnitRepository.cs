

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;

using PharmCare.DTO.UnitModule;

namespace PharmCare.BLL.Repositories.UnitModule
{
    public class UnitRepository : IUnitRepository
    {
        private ApplicationDbContext context;

        private readonly IMapper mapper;
        public UnitRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<UnitDTO> Create(UnitDTO unitDTO)
        {
            try
            {
                unitDTO.Id = Guid.NewGuid();

                unitDTO.CreateDate = DateTime.Now;

                var shelf = mapper.Map<Unit>(unitDTO);

                context.Units.Add(shelf);

                await context.SaveChangesAsync();

                return unitDTO;
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

                var unit = await context.Units.FindAsync(Id);

                if (unit != null)
                {
                    context.Units.Remove(unit);

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
        public async Task<List<UnitDTO>> GetAll()
        {
            try
            {
                var shelves = (from s in context.Units

                               join u in context.AppUsers on s.CreatedBy equals u.Id

                               select new UnitDTO
                               {
                                   Id = s.Id,

                                   Name = s.Name,

                                   UnitValue = s.UnitValue,

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
        public async Task<UnitDTO> GetById(Guid Id)
        {
            try
            {
                var shelf = (from s in context.Units

                             join u in context.AppUsers on s.CreatedBy equals u.Id

                             where s.Id == Id

                             select new UnitDTO
                             {
                                 Id = s.Id,

                                 Name = s.Name,

                                 UnitValue = s.UnitValue,

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
        public async Task<UnitDTO> Update(UnitDTO unitDTO)
        {
            try
            {
                var unit = await context.Units.FindAsync(unitDTO.Id);

                if (unit != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        unit.Name = unitDTO.Name;

                        unit.UnitValue = unitDTO.UnitValue;              

                        unit.UpdatedBy = unitDTO.UpdatedBy;

                        unit.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return unitDTO;
                }

                return null;              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(UnitDTO unitDTO)
        {
            try
            {
                bool result = await context.Units.AnyAsync(p => p.Name == unitDTO.Name & p.UnitValue==unitDTO.UnitValue);

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
