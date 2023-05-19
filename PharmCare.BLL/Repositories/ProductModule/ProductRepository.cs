using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.ProductModule;


namespace PharmCare.BLL.Repositories.ProductModule
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public ProductRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<ProductDTO> Create(ProductDTO productDTO)
        {
            try
            {
                productDTO.Id = Guid.NewGuid();

                productDTO.CreateDate = DateTime.Now;

                var product = mapper.Map<Product>(productDTO);

                context.Products.Add(product);

                var stock = new Stock
                {
                    Id = Guid.NewGuid(),

                    MedicineId = productDTO.Id,

                    CostPrice = 0,

                    SellingPrice = 0,

                    Quantity = 0,

                    CreatedBy = productDTO.CreatedBy,

                    CreateDate = DateTime.Now,
                };
                context.Stocks.Add(stock);

                await context.SaveChangesAsync();

                return productDTO;
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

                var product = await context.Products.FindAsync(Id);

                if (product != null)
                {
                    context.Products.Remove(product);

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
        public async Task<List<ProductDTO>> GetAll()
        {
            try
            {
                var products = (from mc in context.Products

                                join u in context.AppUsers on mc.CreatedBy equals u.Id

                                select new ProductDTO
                                {
                                    Id = mc.Id,

                                    Name = mc.Name,

                                    CreateDate = mc.CreateDate,

                                    CreatedBy = mc.CreatedBy,

                                    CreatedByName = u.FirstName + " " + u.LastName,

                                }).ToListAsync();

                return await products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ProductDTO> GetById(Guid Id)
        {
            try
            {
                var product = (from mc in context.Products

                               join u in context.AppUsers on mc.CreatedBy equals u.Id

                               where mc.Id == Id

                               select new ProductDTO
                               {
                                   Id = mc.Id,

                                   Name = mc.Name,

                                   CreateDate = mc.CreateDate,

                                   CreatedBy = mc.CreatedBy,

                                   CreatedByName = u.FirstName + " " + u.LastName,

                               }).FirstOrDefaultAsync();

                return await product;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ProductDTO> Update(ProductDTO productDTO)
        {
            try
            {
                var product = await context.Products.FindAsync(productDTO.Id);

                if (product != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        product.Name = productDTO.Name;

                        product.UpdatedBy = productDTO.UpdatedBy;

                        product.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return productDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(ProductDTO productDTO)
        {
            try
            {
                bool result = await context.Products.AnyAsync(p => p.Name == productDTO.Name);

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
