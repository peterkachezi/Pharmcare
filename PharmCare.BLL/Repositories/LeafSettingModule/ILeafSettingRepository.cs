using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.LeafSettingModule;

namespace PharmCare.BLL.Repositories.LeafSettingModule
{
    public interface ILeafSettingRepository
    {
        Task<LeafSettingDTO> Create(LeafSettingDTO leafSettingDTO);
        Task<LeafSettingDTO> Update(LeafSettingDTO leafSettingDTO);
        Task<bool> Delete(Guid Id);
        Task<List<LeafSettingDTO>> GetAll();
        Task<LeafSettingDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(LeafSettingDTO leafSettingDTO);
    }
}