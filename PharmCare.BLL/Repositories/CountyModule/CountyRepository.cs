using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.CountyModule;
using PharmCare.DTO.LeafSettingModule;

namespace PharmCare.BLL.Repositories.CountyModule
{
    public class CountyRepository : ICountyRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public CountyRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<CountyDTO> Create(CountyDTO countyDTO)
        {
            try
            {
                countyDTO.Id = Guid.NewGuid();

                countyDTO.CreateDate = DateTime.Now;

                var data = mapper.Map<County>(countyDTO);

                context.Counties.Add(data);

                await context.SaveChangesAsync();

                return countyDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<List<CountyDTO>> GetAllCounties()
        {
            try
            {
                var county = await context.Counties.ToListAsync();

                var counties = mapper.Map<List<CountyDTO>>(county).OrderByDescending(x=>x.CreateDate).ToList();

                return counties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<CountyDTO> GetAllCountyById(Guid Id)
        {
            try
            {
                var data = await context.Counties.FindAsync(Id);

                var county = mapper.Map<CountyDTO>(data);

                return county;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<SubCountyDTO> CreateSubCounty(SubCountyDTO subCountyDTO)
        {
            try
            {
                subCountyDTO.Id = Guid.NewGuid();

                subCountyDTO.CreateDate = DateTime.Now;

                var data = mapper.Map<SubCounty>(subCountyDTO);

                context.SubCounties.Add(data);

                await context.SaveChangesAsync();

                return subCountyDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<List<SubCountyDTO>> GetAllSubCounties()
        {
            try
            {
                var data = await context.SubCounties.ToListAsync();

                var subCounties = mapper.Map<List<SubCountyDTO>>(data).OrderByDescending(x=>x.CreateDate).ToList();

                return subCounties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<SubCountyDTO> GetAllSubCountyById(Guid Id)
        {
            try
            {
                var data = await context.SubCounties.FindAsync(Id);

                var subCounty = mapper.Map<SubCountyDTO>(data);

                return subCounty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfCountyExist(CountyDTO countyDTO)
        {
            try
            {
                bool result = await context.Counties.AnyAsync(p => p.Name == countyDTO.Name);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<bool> CheckIfSubCountyExist(SubCountyDTO subCountyDTO)
        {
            try
            {
                bool result = await context.SubCounties.AnyAsync(p => p.Name == subCountyDTO.Name);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<bool> DeleteCounty(Guid Id)
        {
            try
            {
                bool result = false;

                var county = await context.Counties.FindAsync(Id);

                if (county != null)
                {
                    context.Counties.Remove(county);

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

        public async Task<bool> DeleteSubCounty(Guid Id)
        {
            try
            {
                bool result = false;

                var subCounty = await context.SubCounties.FindAsync(Id);

                if (subCounty != null)
                {
                    context.SubCounties.Remove(subCounty);

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

        public async Task<CountyDTO> UpdateCounty(CountyDTO countyDTO)
        {
            try
            {
                var county = await context.Counties.FindAsync(countyDTO.Id);

                if (county != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        county.Name = countyDTO.Name;

                        county.UpdatedBy = countyDTO.UpdatedBy;

                        county.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return countyDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<SubCountyDTO> UpdateSubCounty(SubCountyDTO subCountyDTO)
        {
            try
            {
                var subCounty = await context.SubCounties.FindAsync(subCountyDTO.Id);

                if (subCounty != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        subCounty.Name = subCountyDTO.Name;

                        subCounty.UpdatedBy = subCountyDTO.UpdatedBy;

                        subCounty.UpdatedDate = DateTime.Now;

                        transaction.Commit();                   
                    }
                    await context.SaveChangesAsync();

                    return subCountyDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
