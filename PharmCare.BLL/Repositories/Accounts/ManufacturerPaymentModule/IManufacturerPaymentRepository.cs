using PharmCare.DTO.Accounts.BankModule;
using PharmCare.DTO.Accounts.ManufacturerPaymentModule;

namespace PharmCare.BLL.Repositories.Accounts.ManufacturerPaymentModule
{
    public interface IManufacturerPaymentRepository
    {
        Task<ManufacturerPaymentDTO> Create(ManufacturerPaymentDTO manufacturerPaymentDTO);
        Task<ManufacturerPaymentDTO> Update(ManufacturerPaymentDTO manufacturerPaymentDTO);
        Task<bool> Delete(Guid Id);
        Task<ManufacturerPaymentDTO> GetById(Guid Id);
        Task<List<ManufacturerPaymentDTO>> GetAll();
        Task<bool> CheckIfRecordExist(ManufacturerPaymentDTO manufacturerPaymentDTO);
    }
}