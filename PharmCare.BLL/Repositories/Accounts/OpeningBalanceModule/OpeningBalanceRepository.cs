

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.OpeningBalanceModule;
using PharmCare.DTO.CategoryModule;

namespace PharmCare.BLL.Repositories.Accounts.OpeningBalanceModule
{
    public class OpeningBalanceRepository : IOpeningBalanceRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public OpeningBalanceRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<bool> CheckIfRecordExist(OpeningBalanceDTO openingBalanceDTO)
        {
            try
            {
                bool result = await context.OpeningBalances.AnyAsync(p => p.VoucherNo == openingBalanceDTO.VoucherNo);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<OpeningBalanceDTO> Create(OpeningBalanceDTO openingBalanceDTO)
        {
            try
            {
                openingBalanceDTO.Id = Guid.NewGuid();

                openingBalanceDTO.CreateDate = DateTime.Now;

                var openingBalance = mapper.Map<OpeningBalance>(openingBalanceDTO);

                context.OpeningBalances.Add(openingBalance);

                await context.SaveChangesAsync();

                return openingBalanceDTO;
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

                var openingBalance = await context.OpeningBalances.FindAsync(Id);

                if (openingBalance != null)
                {
                    context.OpeningBalances.Remove(openingBalance);

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
        public async Task<List<OpeningBalanceDTO>> GetAll()
        {
            try
            {
                var openingBalance = (from op in context.OpeningBalances

                                  join u in context.AppUsers on op.CreatedBy equals u.Id

                                  select new OpeningBalanceDTO
                                  {
                                      Id = op.Id,

                                      Amount = op.Amount,

                                      OpeningDate = op.OpeningDate,

                                      VoucherNo = op.VoucherNo,

                                      AccountHead = op.AccountHead,

                                      Remarks = op.Remarks,

                                      CreateDate = op.CreateDate,

                                      CreatedBy = op.CreatedBy,

                                      CreatedByName = u.FirstName + " " + u.LastName,

                                  }).ToListAsync();

                return await openingBalance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<OpeningBalanceDTO> GetById(Guid Id)
        {
            try
            {
                var openingBalance = (from op in context.OpeningBalances

                                      join u in context.AppUsers on op.CreatedBy equals u.Id

                                      where op.Id == Id

                                      select new OpeningBalanceDTO
                                      {
                                          Id = op.Id,

                                          Amount = op.Amount,

                                          OpeningDate = op.OpeningDate,

                                          VoucherNo = op.VoucherNo,

                                          AccountHead = op.AccountHead,

                                          Remarks = op.Remarks,

                                          CreateDate = op.CreateDate,

                                          CreatedBy = op.CreatedBy,

                                          CreatedByName = u.FirstName + " " + u.LastName,

                                      }).FirstOrDefaultAsync();

                return await openingBalance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<OpeningBalanceDTO> Update(OpeningBalanceDTO openingBalanceDTO)
        {
            try
            {
                var openingBalance = await context.OpeningBalances.FindAsync(openingBalanceDTO.Id);

                if (openingBalance != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        openingBalance.Amount = openingBalanceDTO.Amount;

                        openingBalance.Remarks = openingBalanceDTO.Remarks;

                        openingBalance.VoucherNo = openingBalanceDTO.VoucherNo;

                        openingBalance.AccountHead = openingBalanceDTO.AccountHead;

                        openingBalance.OpeningDate = openingBalanceDTO.OpeningDate;

                        openingBalance.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return openingBalanceDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
