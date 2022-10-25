

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.BankModule;
using PharmCare.DTO.LeafSettingModule;
using System.Collections.Generic;

namespace PharmCare.BLL.Repositories.Accounts.BankModule
{
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public BankRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<BankDTO> Create(BankDTO bankDTO)
        {
            try
            {
                bankDTO.Id = Guid.NewGuid();

                bankDTO.CreateDate = DateTime.Now;

                var bank = mapper.Map<Bank>(bankDTO);

                context.Banks.Add(bank);

                await context.SaveChangesAsync();

                return bankDTO;
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

                var bank = await context.Banks.FindAsync(Id);

                if (bank != null)
                {
                    context.Banks.Remove(bank);

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
        public async Task<List<BankDTO>> GetAll()
        {
            try
            {
                var data = await context.Banks.ToListAsync();

                var banks = mapper.Map<List<BankDTO>>(data);

                return  banks;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<BankDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Banks.FindAsync(Id);

                var bank = mapper.Map<BankDTO>(data);

                return bank;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<BankDTO> Update(BankDTO bankDTO)
        {
            try
            {
                var bank = await context.Banks.FindAsync(bankDTO.Id);

                if (bank != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        bank.Name = bankDTO.Name;

                        bank.UpdatedBy = bankDTO.UpdatedBy;

                        bank.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return bankDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(BankDTO bankDTO)
        {
            try
            {
                bool result = await context.Banks.AnyAsync(p => p.Name == bankDTO.Name);

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
