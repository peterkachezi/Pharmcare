using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.ProductModule;

namespace PharmCare.BLL.Repositories.ProductModule
{
    public interface IProductRepository
    {
        Task<ProductDTO> Create(ProductDTO productDTO);
        Task<ProductDTO> Update(ProductDTO productDTO);
        Task<bool> Delete(Guid Id);
        Task<List<ProductDTO>> GetAll();
        Task<ProductDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(ProductDTO productDTO);
    }
}