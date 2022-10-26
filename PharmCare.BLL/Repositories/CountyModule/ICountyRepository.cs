

using PharmCare.DTO.CountryModule;
using PharmCare.DTO.CountyModule;

namespace PharmCare.BLL.Repositories.CountyModule
{
    public interface ICountyRepository
    {
        Task<bool> CheckIfCountyExist(CountyDTO countyDTO);
        Task<bool> CheckIfSubCountyExist(SubCountyDTO subCountyDTO);
        Task<CountyDTO> Create(CountyDTO countyDTO);
        Task<SubCountyDTO> CreateSubCounty(SubCountyDTO subCountyDTO);
        Task<bool> DeleteCounty(Guid Id);
        Task<bool> DeleteSubCounty(Guid Id);
        Task<List<CountyDTO>> GetAllCounties();
        Task<CountyDTO> GetAllCountyById(Guid Id);
        Task<List<SubCountyDTO>> GetAllSubCounties();
        Task<SubCountyDTO> GetAllSubCountyById(Guid Id);
        Task<CountyDTO> UpdateCounty(CountyDTO countyDTO);
        Task<SubCountyDTO> UpdateSubCounty(SubCountyDTO subCountyDTO);
    }
}