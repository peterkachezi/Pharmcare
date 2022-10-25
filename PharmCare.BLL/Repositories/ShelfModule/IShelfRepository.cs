using PharmCare.DTO.ShelfModule;

namespace PharmCare.BLL.Repositories.ShelfModule
{
    public interface IShelfRepository
    {
        Task<ShelfDTO> Create(ShelfDTO shelfDTO);
        Task<ShelfDTO> Update(ShelfDTO shelfDTO);
        Task<bool> Delete(Guid Id);
        Task<List<ShelfDTO>> GetAll();
        Task<ShelfDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(ShelfDTO shelfDTO);
    }
}
