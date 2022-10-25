using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.ProductTypeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.BLL.Repositories.ProductTypeModule
{
    public  class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public ProductTypeRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<ProductTypeDTO> Create(ProductTypeDTO productTypeDTO)
        {
            try
            {
                productTypeDTO.Id = Guid.NewGuid();

                productTypeDTO.CreateDate = DateTime.Now;

                var productType = mapper.Map<ProductType>(productTypeDTO);

                context.ProductTypes.Add(productType);

                await context.SaveChangesAsync();

                return productTypeDTO;
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

                var productType = await context.ProductTypes.FindAsync(Id);

                if (productType != null)
                {
                    context.ProductTypes.Remove(productType);

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
        public async Task<List<ProductTypeDTO>> GetAll()
        {
            try
            {
                var categories = (from mc in context.ProductTypes

                                  join u in context.AppUsers on mc.CreatedBy equals u.Id

                                  select new ProductTypeDTO
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
        public async Task<ProductTypeDTO> GetById(Guid Id)
        {
            try
            {
                var productType = (from mc in context.ProductTypes

                                join u in context.AppUsers on mc.CreatedBy equals u.Id

                                where mc.Id == Id

                                select new ProductTypeDTO
                                {
                                    Id = mc.Id,

                                    Name = mc.Name,                                

                                    CreateDate = mc.CreateDate,

                                    CreatedBy = mc.CreatedBy,

                                    CreatedByName = u.FirstName + " " + u.LastName,

                                }).FirstOrDefaultAsync();

                return await productType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ProductTypeDTO> Update(ProductTypeDTO productTypeDTO)
        {
            try
            {
                var productType = await context.ProductTypes.FindAsync(productTypeDTO.Id);

                if (productType != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        productType.Name = productTypeDTO.Name;

                        productType.UpdatedBy = productTypeDTO.UpdatedBy;

                        productType.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return productTypeDTO;
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
