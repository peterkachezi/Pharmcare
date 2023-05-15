using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.MedicalConditionModule;

namespace PharmCare.BLL.Repositories.MedicalConditionModule
{
    public class MedicalConditionRepository : IMedicalConditionRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public MedicalConditionRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<MedicalConditionDTO> Create(MedicalConditionDTO medicalConditionDTO)
        {
            try
            {
                medicalConditionDTO.Id = Guid.NewGuid();

                medicalConditionDTO.CreateDate = DateTime.Now;

                var medicalCondition = mapper.Map<MedicalCondition>(medicalConditionDTO);

                context.MedicalConditions.Add(medicalCondition);

                await context.SaveChangesAsync();

                return medicalConditionDTO;
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

                var medicalCondition = await context.MedicalConditions.FindAsync(Id);

                if (medicalCondition != null)
                {
                    medicalCondition.Status = 2;

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

        public async Task<bool> PermamnetDelete(Guid Id)
        {
            try
            {
                bool result = false;

                var medicalCondition = await context.MedicalConditions.FindAsync(Id);

                if (medicalCondition != null)
                {
                    context.MedicalConditions.Remove(medicalCondition);

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
        public async Task<List<MedicalConditionDTO>> GetAll()
        {
            try
            {
                var data = await context.MedicalConditions.Where(x => x.Status != 3).ToListAsync();

                var medical_Condition = mapper.Map<List<MedicalConditionDTO>>(data);

                return medical_Condition;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<MedicalConditionDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.MedicalConditions.FindAsync(Id);

                var medical_Condition = mapper.Map<MedicalConditionDTO>(data);

                return medical_Condition;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<MedicalConditionDTO> Update(MedicalConditionDTO medicalConditionDTO)
        {
            try
            {
                var medicalCondition = await context.MedicalConditions.FindAsync(medicalConditionDTO.Id);

                if (medicalCondition != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        medicalCondition.Name = medicalConditionDTO.Name;

                        medicalCondition.UpdatedBy = medicalConditionDTO.UpdatedBy;

                        medicalCondition.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return medicalConditionDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(MedicalConditionDTO medicalConditionDTO)
        {
            try
            {
                bool result = await context.MedicalConditions.AnyAsync(p => p.Name == medicalConditionDTO.Name);

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
