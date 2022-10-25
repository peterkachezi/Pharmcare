
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.ManufacturerPaymentModule;

namespace PharmCare.BLL.Repositories.Accounts.ManufacturerPaymentModule
{
    public class ManufacturerPaymentRepository : IManufacturerPaymentRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public ManufacturerPaymentRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<ManufacturerPaymentDTO> Create(ManufacturerPaymentDTO manufacturerPaymentDTO)
        {
            try
            {
                manufacturerPaymentDTO.Id = Guid.NewGuid();

                manufacturerPaymentDTO.CreateDate = DateTime.Now;

                var manufacturerPayment = mapper.Map<ManufacturerPayment>(manufacturerPaymentDTO);

                context.ManufacturerPayments.Add(manufacturerPayment);

                await context.SaveChangesAsync();

                return manufacturerPaymentDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var manufacturerPayment = await context.ManufacturerPayments.FindAsync(Id);

                if (manufacturerPayment != null)
                {
                    context.ManufacturerPayments.Remove(manufacturerPayment);

                    await context.SaveChangesAsync();

                    return true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<List<ManufacturerPaymentDTO>> GetAll()
        {
            try
            {
                var data = await context.ManufacturerPayments.ToListAsync();

                var manufacturerPayment = mapper.Map<List<ManufacturerPaymentDTO>>(data);

                return manufacturerPayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ManufacturerPaymentDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.ManufacturerPayments.FindAsync(Id);

                var manufacturerPayment = mapper.Map<ManufacturerPaymentDTO>(data);

                return manufacturerPayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ManufacturerPaymentDTO> Update(ManufacturerPaymentDTO  manufacturerPaymentDTO)
        {
            try
            {
                var manufacturerPayment = await context.ManufacturerPayments.FindAsync(manufacturerPaymentDTO.Id);

                if (manufacturerPayment != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        manufacturerPayment.SupplierId = manufacturerPaymentDTO.SupplierId;

                        manufacturerPayment.UpdatedBy = manufacturerPaymentDTO.UpdatedBy;

                        manufacturerPayment.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return manufacturerPaymentDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(ManufacturerPaymentDTO manufacturerPaymentDTO)
        {
            try
            {
                bool result = await context.ManufacturerPayments.AnyAsync(p => p.SupplierId == manufacturerPaymentDTO.SupplierId);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
