using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;

namespace PharmCare.BLL.Repositories.PaymentModule
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext context;
        public PaymentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<OnlinePayment> Create(OnlinePayment onlinePayment)
        {
            try
            {
                context.OnlinePayments.Add(onlinePayment);

                await context.SaveChangesAsync();

                return onlinePayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
    }
}
