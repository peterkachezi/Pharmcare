using PharmCare.DTO.Accounts.OpeningBalanceModule;
using PharmCare.DTO.CategoryModule;

namespace PharmCare.BLL.Repositories.Accounts.OpeningBalanceModule
{
    public interface IOpeningBalanceRepository
    {
        Task<OpeningBalanceDTO> Create(OpeningBalanceDTO openingBalanceDTO);    
        Task<OpeningBalanceDTO> Update(OpeningBalanceDTO openingBalanceDTO);    
        Task<bool> Delete(Guid Id);    
        Task<OpeningBalanceDTO> GetById(Guid Id);    
        Task<List<OpeningBalanceDTO>> GetAll();
        Task<bool> CheckIfRecordExist(OpeningBalanceDTO openingBalanceDTO);
    }
}