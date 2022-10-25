
using PharmCare.DTO.UnitModule;

namespace PharmCare.BLL.Repositories.UnitModule
{
    public interface IUnitRepository
    {
        Task<UnitDTO> Create(UnitDTO unitDTO);
        Task<UnitDTO> Update(UnitDTO unitDTO);
        Task<bool> Delete(Guid Id);
        Task<List<UnitDTO>> GetAll();
        Task<UnitDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(UnitDTO unitDTO);
    }
}