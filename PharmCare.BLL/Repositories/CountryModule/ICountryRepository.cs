using PharmCare.DTO.CountryModule;

namespace PharmCare.BLL.Repositories.CountryModule
{
    public interface ICountryRepository
    {
        Task<CountryDTO> Create(CountryDTO countryDTO);
        Task<CountryDTO> Update(CountryDTO countryDTO);
        Task<List<CountryDTO>> GetAll();
        Task<CountryDTO> GetById(Guid Id);
        Task<bool> Delete(Guid Id);
        Task<bool> CheckIfRecordExist(CountryDTO countryDTO);
    }
}