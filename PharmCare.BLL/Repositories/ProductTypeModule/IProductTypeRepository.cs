using PharmCare.DTO.PatientModule;
using PharmCare.DTO.ProductTypeModule;

namespace PharmCare.BLL.Repositories.ProductTypeModule
{
    public interface IProductTypeRepository
    {
        Task<ProductTypeDTO> Create(ProductTypeDTO productTypeDTO);
        Task<ProductTypeDTO> Update(ProductTypeDTO productTypeDTO);
        Task<bool> Delete(Guid Id);
        Task<List<ProductTypeDTO>> GetAll();
        Task<ProductTypeDTO> GetById(Guid Id);
    
    }
}