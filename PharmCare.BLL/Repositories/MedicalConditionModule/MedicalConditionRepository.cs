using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.MedicalConditionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var categories = (from mc in context.MedicalConditions

                                  join u in context.AppUsers on mc.CreatedBy equals u.Id

                                  select new MedicalConditionDTO
                                  {
                                      Id = mc.Id,

                                      Name = mc.Name,                                   

                                      CreateDate = mc.CreateDate,

                                      CreatedBy = mc.CreatedBy,

                                      CreatedByName = u.FirstName + " " + u.LastName,

                                  }).ToListAsync();

                return await categories;
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
                var medicalCondition = (from mc in context.MedicalConditions

                                join u in context.AppUsers on mc.CreatedBy equals u.Id

                                where mc.Id == Id

                                select new MedicalConditionDTO
                                {
                                    Id = mc.Id,

                                    Name = mc.Name,

                                    CreateDate = mc.CreateDate,

                                    CreatedBy = mc.CreatedBy,

                                    CreatedByName = u.FirstName + " " + u.LastName,

                                }).FirstOrDefaultAsync();

                return await medicalCondition;
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
