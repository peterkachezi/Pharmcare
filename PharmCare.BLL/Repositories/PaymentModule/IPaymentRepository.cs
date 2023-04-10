using PharmCare.DAL.Models;

namespace PharmCare.BLL.Repositories.PaymentModule
{
    public interface IPaymentRepository
    {
        Task<OnlinePayment> Create(OnlinePayment onlinePayment);
    }
}