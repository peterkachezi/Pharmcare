using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.LeafSettingModule;

namespace PharmCare.BLL.Repositories.LeafSettingModule
{
    public class LeafSettingRepository : ILeafSettingRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public LeafSettingRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<LeafSettingDTO> Create(LeafSettingDTO leafSettingDTO)
        {
            try
            {
                leafSettingDTO.Id = Guid.NewGuid();

                leafSettingDTO.CreateDate = DateTime.Now;

                var leafSetting = mapper.Map<LeafSetting>(leafSettingDTO);

                context.LeafSettings.Add(leafSetting);

                await context.SaveChangesAsync();

                return leafSettingDTO;
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

                var leafSetting = await context.LeafSettings.FindAsync(Id);

                if (leafSetting != null)
                {
                    context.LeafSettings.Remove(leafSetting);

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
        public async Task<List<LeafSettingDTO>> GetAll()
        {
            try
            {
                var categories = (from mc in context.LeafSettings

                                  join u in context.AppUsers on mc.CreatedBy equals u.Id

                                  select new LeafSettingDTO
                                  {
                                      Id = mc.Id,

                                      LeafType = mc.LeafType,

                                      TotalNumberPerBox = mc.TotalNumberPerBox,

                                      Status = mc.Status,

                                      CreateDate = mc.CreateDate,

                                      CreatedBy = mc.CreatedBy,

                                      CreatedByName = u.FirstName + " " + u.LastName,

                                  }).ToListAsync();

                return await categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<LeafSettingDTO> GetById(Guid Id)
        {
            try
            {
                var leafSetting = (from mc in context.LeafSettings

                                   join u in context.AppUsers on mc.CreatedBy equals u.Id

                                   where mc.Id == Id

                                   select new LeafSettingDTO
                                   {
                                       Id = mc.Id,

                                       LeafType = mc.LeafType,

                                       TotalNumberPerBox = mc.TotalNumberPerBox,

                                       Status = mc.Status,

                                       CreateDate = mc.CreateDate,

                                       CreatedBy = mc.CreatedBy,

                                       CreatedByName = u.FirstName + " " + u.LastName,

                                   }).FirstOrDefaultAsync();

                return await leafSetting;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<LeafSettingDTO> Update(LeafSettingDTO leafSettingDTO)
        {
            try
            {
                var leafSetting = await context.LeafSettings.FindAsync(leafSettingDTO.Id);

                if (leafSetting != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        leafSetting.LeafType = leafSettingDTO.LeafType;

                        leafSetting.TotalNumberPerBox = leafSettingDTO.TotalNumberPerBox;

                        leafSetting.UpdatedBy = leafSettingDTO.UpdatedBy;

                        leafSetting.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return leafSettingDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(LeafSettingDTO leafSettingDTO)
        {
            try
            {
                bool result = await context.LeafSettings.AnyAsync(p => p.LeafType == leafSettingDTO.LeafType & p.TotalNumberPerBox == leafSettingDTO.TotalNumberPerBox);

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
