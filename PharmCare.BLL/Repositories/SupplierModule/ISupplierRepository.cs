using PharmCare.DTO.SupplierModule;

namespace PharmCare.BLL.Repositories.SupplierModule
{
    public interface ISupplierRepository
    {
        Task<SupplierDTO> Create(SupplierDTO supplierDTO);
        Task<SupplierDTO> Update(SupplierDTO supplierDTO);
        Task<SupplierDTO> GetById(Guid Id);
        Task<List<SupplierDTO>> GetAll();
        Task<bool> Delete(Guid Id);
        Task<bool> CheckIfRecordExist(SupplierDTO supplierDTO);
    }
}