using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.MedicalConditionModule;

namespace PharmCare.BLL.Repositories.MedicalConditionModule
{
    public interface IMedicalConditionRepository
    {
        Task<MedicalConditionDTO> Create(MedicalConditionDTO medicalConditionDTO);
        Task<MedicalConditionDTO> Update(MedicalConditionDTO medicalConditionDTO);
        Task<bool> Delete(Guid Id);
        Task<List<MedicalConditionDTO>> GetAll();
        Task<MedicalConditionDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(MedicalConditionDTO medicalConditionDTO);
    }
}