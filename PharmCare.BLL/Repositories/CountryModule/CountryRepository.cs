using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.CountryModule;


namespace PharmCare.BLL.Repositories.CountryModule
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        public CountryRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public async Task<CountryDTO> Create(CountryDTO countryDTO)
        {
            try
            {
                countryDTO.Id = Guid.NewGuid();

                countryDTO.CreateDate = DateTime.Now;

                var patient = mapper.Map<Country>(countryDTO);

                context.Countries.Add(patient);

                await context.SaveChangesAsync();

                return countryDTO;
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

                var county = await context.Countries.FindAsync(Id);

                if (county != null)
                {
                    context.Countries.Remove(county);

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
        public async Task<CountryDTO> Update(CountryDTO countryDTO)
        {
            try
            {
                var patient = await context.Countries.FindAsync(countryDTO.Id);

                if (patient != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        patient.Name = countryDTO.Name;

                        patient.UpdatedDate = DateTime.Now;

                        patient.UpdatedBy = countryDTO.UpdatedBy;

                        transaction.Commit();
                    }
                    await context.SaveChangesAsync();

                    return countryDTO;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<List<CountryDTO>> GetAll()
        {
            try
            {
                var data = await context.Countries.OrderByDescending(x => x.Name).ToListAsync();

                var patient = mapper.Map<List<CountryDTO>>(data);

                return patient;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<CountryDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Countries.FindAsync(Id);

                var customers = mapper.Map<CountryDTO>(data);

                return customers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(CountryDTO countryDTO)
        {
            try
            {
                bool result = await context.Countries.AnyAsync(p => p.Name == countryDTO.Name);

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
