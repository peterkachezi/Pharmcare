
using PharmCare.DTO.MedicineModule;

namespace PharmCare.BLL.Repositories.MedecineModule
{
    public interface IMedicineRepository
    {
        Task<MedicineDTO> Create(MedicineDTO medicineDTO);
        Task<MedicineDTO> Update(MedicineDTO medicineDTO);
        Task<bool> Delete(Guid Id);
        Task<List<MedicineDTO>> GetAll();
        Task<MedicineDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(MedicineDTO medicineDTO);
        Task<MedicineDTO> GetStockDetailsById(Guid Id);
        Task<List<MedicineDTO>> GetAllOutOfStockProducts();
    }
}