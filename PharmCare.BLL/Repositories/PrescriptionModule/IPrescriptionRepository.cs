using PharmCare.DTO.PrescriptionModule;
using PharmCare.DTO.SalesModule;

namespace PharmCare.BLL.Repositories.PrescriptionModule
{
    public interface IPrescriptionRepository
    {
        Task<PrescriptionDTO> Create(PrescriptionDTO prescriptionDTO);
        Task<bool> Delete(Guid Id);
        Task<bool> DeleteDetails(Guid Id);
        Task<bool> UnDoIssueMedicine(Guid Id);
        Task<SalesDetailsDTO> IssueMedicine(SalesDetailsDTO salesDetailsDTO);
        Task<List<PrescriptionDTO>> GetAll();
        List<PrescriptionDetailDTO> GetAllPrescriptionDetailsById(Guid Id);
        Task<PrescriptionDetailDTO> GetById(Guid Id);
        Task<PrescriptionDTO> GetPrescriptionById(Guid Id);
    }
}