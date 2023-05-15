
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
                    category.Status = 2;

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

        public async Task<bool> PermanentDelete(Guid Id)
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
                var data = await context.Categories.Where(x => x.Status != 2).ToListAsync();

                var categories = mapper.Map<List<CategoryDTO>>(data);

                return categories;
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
                var data = await context.Categories.FindAsync(Id);

                var categories = mapper.Map<CategoryDTO>(data);

                return categories;
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
