using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.MedicineModule;


namespace PharmCare.BLL.Repositories.MedecineModule
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public MedicineRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;

        }
        public async Task<MedicineDTO> Create(MedicineDTO medicineDTO)
        {
            try
            {
                medicineDTO.Id = Guid.NewGuid();

                medicineDTO.CreateDate = DateTime.Now;

                medicineDTO.MedicalConditionId = medicineDTO.MedicalConditionId;

                var medicine = mapper.Map<Medicine>(medicineDTO);

                context.Medicines.Add(medicine);

                await context.SaveChangesAsync();

                return medicineDTO;
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

                var category = await context.Medicines.FindAsync(Id);

                if (category != null)
                {
                    context.Medicines.Remove(category);

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
        public async Task<List<MedicineDTO>> GetAll()
        {
            try
            {
                var medicines = (from m in context.Medicines

                                 join shelf in context.Shelves on m.ShelfId equals shelf.Id into tbllshelf

                                 from newshelf in tbllshelf.DefaultIfEmpty()

                                 join cat in context.Categories on m.CategoryId equals cat.Id into tblcat

                                 from newCat in tblcat.DefaultIfEmpty()

                                 join stocks in context.Stocks on m.Id equals stocks.MedicineId into tblStock

                                 from newStock in tblStock.DefaultIfEmpty()

                                 join medCond in context.MedicalConditions on m.MedicalConditionId equals medCond.Id into tblmedCond

                                 from newMedCond in tblmedCond.DefaultIfEmpty()

                                 join unit in context.Units on m.UnitId equals unit.Id

                                 select new MedicineDTO
                                 {
                                     Id = m.Id,

                                     MedicalConditionId = m.MedicalConditionId,

                                     Name = m.Name,

                                     ShelfId = m.ShelfId,

                                     ShelfName = newshelf.Name == null ? "" : newshelf.Name,

                                     MedicalConditionName = newMedCond.Name == null ? "" : newMedCond.Name,

                                     Description = m.Description,

                                     CategoryId = m.CategoryId,

                                     CategoryName = newCat.Name == null ? "" : newCat.Name,

                                     ManufacturerPrice = m.ManufacturerPrice,

                                     SellingPrice = m.SellingPrice,

                                     Status = m.Status,

                                     CreateDate = m.CreateDate,

                                     CreatedBy = m.CreatedBy,

                                     UnitId = m.UnitId,

                                     UnitName = unit.UnitValue + " " + unit.Name,

                                     Quantity = newStock.Quantity == null ? 0 : newStock.Quantity,

                                     StockDate = newStock.CreateDate == null ? DateTime.Now : newStock.CreateDate,

                                     StockId = newStock.Id == null ? Guid.NewGuid(): newStock.Id,

                                 }).OrderByDescending(x => x.CreateDate).ToListAsync();

                return await medicines;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<List<MedicineDTO>> GetAllOutOfStockProducts()
        {
            try
            {
                var medicines = (from m in context.Medicines

                                 join stocks in context.Stocks on m.Id equals stocks.MedicineId into tblStock

                                 from newStock in tblStock.DefaultIfEmpty()

                                 where newStock.Quantity == 0

                                 select new MedicineDTO
                                 {
                                     Id = m.Id,

                                     MedicalConditionId = m.MedicalConditionId,

                                     Name = m.Name,

                                     ShelfId = m.ShelfId,

                                     Description = m.Description,

                                     CategoryId = m.CategoryId,

                                     ManufacturerPrice = m.ManufacturerPrice,

                                     SellingPrice = m.SellingPrice,

                                     Status = m.Status,

                                     CreateDate = m.CreateDate,

                                     CreatedBy = m.CreatedBy,

                                     UnitId = m.UnitId,

                                     Quantity = newStock.Quantity == null ? 0 : newStock.Quantity,

                                 }).ToListAsync();

                return await medicines;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<MedicineDTO> GetById(Guid Id)
        {
            try
            {
                var medicines = (from m in context.Medicines

                                 join shelf in context.Shelves on m.ShelfId equals shelf.Id into tbllshelf

                                 from newshelf in tbllshelf.DefaultIfEmpty()

                                 join cat in context.Categories on m.CategoryId equals cat.Id into tblcat

                                 from newCat in tblcat.DefaultIfEmpty()

                                 join stocks in context.Stocks on m.Id equals stocks.MedicineId into tblStock

                                 from newStock in tblStock.DefaultIfEmpty()

                                 join medCond in context.MedicalConditions on m.MedicalConditionId equals medCond.Id into tblmedCond

                                 from newMedCond in tblmedCond.DefaultIfEmpty()

                                 join unit in context.Units on m.UnitId equals unit.Id

                                 where m.Id == Id

                                 select new MedicineDTO
                                 {
                                     Id = m.Id,

                                     MedicalConditionId = m.MedicalConditionId,

                                     Name = m.Name,

                                     ShelfId = m.ShelfId,

                                     ShelfName = newshelf.Name == null ? "" : newshelf.Name,

                                     MedicalConditionName = newMedCond.Name == null ? "" : newMedCond.Name,

                                     Description = m.Description,

                                     CategoryId = m.CategoryId,

                                     CategoryName = newCat.Name == null ? "" : newCat.Name,

                                     ManufacturerPrice = m.ManufacturerPrice,

                                     SellingPrice = m.SellingPrice,

                                     Status = m.Status,

                                     CreateDate = m.CreateDate,

                                     CreatedBy = m.CreatedBy,

                                     UnitId = m.UnitId,

                                     UnitName = unit.UnitValue + " " + unit.Name,

                                     Quantity = newStock.Quantity == null ? 0 : newStock.Quantity,                                     

                                 }).FirstOrDefaultAsync();

                return await medicines;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<MedicineDTO> GetByStockId(Guid Id)
        {
            try
            {

                var medicines = (from s in context.Stocks.Where(x => x.Id == Id)

                                 join m in context.Medicines on s.MedicineId equals m.Id                                                          

                                 select new MedicineDTO
                                 {
                                     Id = m.Id,

                                     Name = m.Name,

                                     StockId = s.Id,

                                     Quantity = s.Quantity,

                                 }).FirstOrDefaultAsync();

                return await medicines;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<MedicineDTO> GetStockDetailsById(Guid Id)
        {
            try
            {
                var medicines = (from m in context.Medicines

                                 join stocks in context.Stocks on m.Id equals stocks.MedicineId into table1

                                 from newStock in table1.DefaultIfEmpty()

                                 where m.Id == Id

                                 select new MedicineDTO
                                 {
                                     Id = m.Id,

                                     MedicalConditionId = m.MedicalConditionId,

                                     Name = m.Name,

                                     ShelfId = m.ShelfId,

                                     Description = m.Description,

                                     CategoryId = m.CategoryId,

                                     ManufacturerPrice = m.ManufacturerPrice,

                                     SellingPrice = m.SellingPrice,

                                     Status = m.Status,

                                     CreateDate = m.CreateDate,

                                     CreatedBy = m.CreatedBy,

                                     Quantity = newStock.Quantity == null ? 0 : newStock.Quantity,

                                 }).FirstOrDefaultAsync();

                return await medicines;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }



        public async Task<MedicineDTO> Update(MedicineDTO medicineDTO)
        {
            try
            {
                var medicine = await context.Medicines.FindAsync(medicineDTO.Id);

                if (medicine != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        medicine.MedicalConditionId = medicineDTO.MedicalConditionId;

                        medicine.Name = medicineDTO.Name;

                        medicine.UnitId = medicineDTO.UnitId;

                        medicine.ShelfId = medicineDTO.ShelfId;

                        medicine.Description = medicineDTO.Description;

                        medicine.CategoryId = medicineDTO.CategoryId;

                        medicine.ManufacturerPrice = medicineDTO.ManufacturerPrice;

                        medicine.SellingPrice = medicineDTO.SellingPrice;

                        medicine.Status = medicineDTO.Status;

                        medicine.UpdatedBy = medicineDTO.UpdatedBy;

                        medicine.Description = medicineDTO.Description;

                        transaction.Commit();

                    }
                    await context.SaveChangesAsync();

                    return medicineDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> CheckIfRecordExist(MedicineDTO medicineDTO)
        {
            try
            {
                bool result = await context.Medicines.AnyAsync(p => p.Name == medicineDTO.Name & p.UnitId == medicineDTO.UnitId);

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
