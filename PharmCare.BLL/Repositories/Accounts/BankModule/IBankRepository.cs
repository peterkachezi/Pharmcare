using PharmCare.DTO.Accounts.BankModule;
using PharmCare.DTO.Accounts.OpeningBalanceModule;

namespace PharmCare.BLL.Repositories.Accounts.BankModule
{
    public interface IBankRepository
    {
        Task<BankDTO> Create(BankDTO bankDTO);
        Task<BankDTO> Update(BankDTO bankDTO);
        Task<bool> Delete(Guid Id);
        Task<BankDTO> GetById(Guid Id);
        Task<List<BankDTO>> GetAll();
        Task<bool> CheckIfRecordExist(BankDTO bankDTO);
    }
}