
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DTO.CategoryModule;
using PharmCare.DAL.Models;
using AutoMapper;



namespace PharmCare.BLL.Repositories.CategoryModule
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public CategoryRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<CategoryDTO> Create(CategoryDTO categoryDTO)
        {
            try
            {
                categoryDTO.Id = Guid.NewGuid();

                categoryDTO.CreateDate = DateTime.Now;

                var category = mapper.Map<Category>(categoryDTO);

                context.Categories.Add(category);

                await context.SaveChangesAsync();

                return categoryDTO;
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

                var category = await context.Categories.FindAsync(Id);

                if (category != null)
                {
                    context.Categories.Remove(category);

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
        public async Task<List<CategoryDTO>> GetAll()
        {
            try
            {
                var categories = (from mc in context.Categories

                                  join u in context.AppUsers on mc.CreatedBy equals u.Id

                                  select new CategoryDTO
                                  {
                                      Id = mc.Id,

                                      Name = mc.Name,

                                      Status = mc.Status,

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
        public async Task<CategoryDTO> GetById(Guid Id)
        {
            try
            {
                var category = (from mc in context.Categories

                                join u in context.AppUsers on mc.CreatedBy equals u.Id

                                where mc.Id == Id

                                select new CategoryDTO
                                {
                                    Id = mc.Id,

                                    Name = mc.Name,

                                    Status = mc.Status,

                                    CreateDate = mc.CreateDate,

                                    CreatedBy = mc.CreatedBy,

                                    CreatedByName = u.FirstName + " " + u.LastName,

                                }).FirstOrDefaultAsync();

                return await category;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<CategoryDTO> Update(CategoryDTO categoryDTO)
        {
            try
            {
                var category = await context.Categories.FindAsync(categoryDTO.Id);

                if (category != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        category.Name = categoryDTO.Name;

                        category.UpdatedBy = categoryDTO.UpdatedBy;

                        category.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return categoryDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(CategoryDTO categoryDTO)
        {
            try
            {
                bool result = await context.Categories.AnyAsync(p => p.Name == categoryDTO.Name);

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
