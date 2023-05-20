
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.SupplierModule;


namespace PharmCare.BLL.Repositories.SupplierModule
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public SupplierRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<SupplierDTO> Create(SupplierDTO supplierDTO)
        {

            try
            {
                var SupplierId = Guid.NewGuid();

                supplierDTO.Id = SupplierId;

                string man_number = PatientNumber.GenerateUniqueNumber();

                supplierDTO.SupplierNo = "M" + "" + man_number;

                var manufacturer = new Supplier
                {
                    Id = supplierDTO.Id,

                    Name = supplierDTO.Name,

                    CountryId = supplierDTO.CountryId,

                    SupplierNo = supplierDTO.SupplierNo,

                    PhoneNumber = supplierDTO.PhoneNumber,

                    Email = supplierDTO.Email,

                    Town = supplierDTO.Town,

                    PhysicalAddress = supplierDTO.PhysicalAddress,

                    CreateDate = DateTime.Now,

                    CreatedBy = supplierDTO.CreatedBy,

                    ProductTypeId = supplierDTO.ProductTypeId,

                };

                context.Suppliers.Add(manufacturer);

                await context.SaveChangesAsync();

                return supplierDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public bool CreateContactPerson(SupplierDTO supplierDTO)
        {
            try
            {
                var contactPerson = new ContactPerson
                {
                    Id = Guid.NewGuid(),

                    SupplierId = supplierDTO.Id,

                    CreateDate = DateTime.Now,

                    FirstName = supplierDTO.ContactFirstName,

                    CreatedBy = supplierDTO.CreatedBy,

                    LastName = supplierDTO.ContactLastName,

                    Email = supplierDTO.ContactEmail,

                    PhoneNumber = supplierDTO.ContactPhoneNumber,

                    CountryId = supplierDTO.ContactCountryId,
                };

                context.ContactPersons.Add(contactPerson);

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }


        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var man = await context.Suppliers.FindAsync(Id);

                if (man != null)
                {
                    man.Status = 2;

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

                var man = await context.Suppliers.FindAsync(Id);

                if (man != null)
                {
                    context.Suppliers.Remove(man);

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

        public async Task<List<SupplierDTO>> GetAll()
        {
            try
            {
                var supplier = (from man in context.Suppliers.Where(x => x.Status != 2)

                                join cont in context.ContactPersons on man.Id equals cont.SupplierId into tblCont

                                from newCont in tblCont.DefaultIfEmpty()

                                join country in context.Countries on man.CountryId equals country.Id into tblcountry

                                from newcountry in tblcountry.DefaultIfEmpty()

                                join prodType in context.ProductTypes on man.ProductTypeId equals prodType.Id into tblProdType

                                from newprodType in tblProdType.DefaultIfEmpty()

                                select new SupplierDTO
                                {
                                    Id = man.Id,

                                    Name = man.Name,

                                    CountryId = man.CountryId,

                                    ProductTypeId = man.ProductTypeId,

                                    ProductTypeName = newprodType.Name == null ? "" : newprodType.Name,

                                    Town = newcountry.Name == null ? "" : newcountry.Name,

                                    SupplierNo = man.SupplierNo,

                                    PhoneNumber = man.PhoneNumber,

                                    Email = man.Email,

                                    PhysicalAddress = man.PhysicalAddress,

                                    CreateDate = man.CreateDate,

                                    CreatedBy = man.CreatedBy,

                                }).ToListAsync();

                return await supplier;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<SupplierDTO> GetById(Guid Id)
        {
            try
            {
                var manufacturer = (from man in context.Suppliers


                                    join country in context.Countries on man.CountryId equals country.Id into tblcountry

                                    from newcountry in tblcountry.DefaultIfEmpty()

                                    join prodType in context.ProductTypes on man.ProductTypeId equals prodType.Id into tblProdType

                                    from newprodType in tblProdType.DefaultIfEmpty()

                                    where man.Id == Id

                                    select new SupplierDTO
                                    {
                                        Id = man.Id,

                                        Name = man.Name,

                                        CountryId = man.CountryId,

                                        ProductTypeId = man.ProductTypeId,

                                        Town = newprodType.Name == null ? "" : newcountry.Name,

                                        ProductTypeName = newcountry.Name == null ? "" : newcountry.Name,

                                        SupplierNo = man.SupplierNo,

                                        PhoneNumber = man.PhoneNumber,

                                        Email = man.Email,

                                        PhysicalAddress = man.PhysicalAddress,

                                        CreateDate = man.CreateDate,

                                        CreatedBy = man.CreatedBy,

                                    }).FirstOrDefaultAsync();

                return await manufacturer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<SupplierDTO> Update(SupplierDTO supplierDTO)
        {
            try
            {
                var manufacturer = await context.Suppliers.FindAsync(supplierDTO.Id);

                if (manufacturer != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        manufacturer.Name = supplierDTO.Name;

                        manufacturer.CountryId = supplierDTO.CountryId;

                        manufacturer.PhoneNumber = supplierDTO.PhoneNumber;

                        manufacturer.Email = supplierDTO.Email;

                        manufacturer.Town = supplierDTO.Town;

                        manufacturer.PhysicalAddress = supplierDTO.PhysicalAddress;

                        manufacturer.UpdatedDate = DateTime.Now;

                        manufacturer.UpdatedBy = supplierDTO.UpdatedBy;

                        manufacturer.CountryId = supplierDTO.CountryId;

                        manufacturer.PhysicalAddress = supplierDTO.PhysicalAddress;

                        manufacturer.ProductTypeId = supplierDTO.ProductTypeId;

                        transaction.Commit();
                    }

                    await context.SaveChangesAsync();

                    UpdateContactPerson(supplierDTO);

                    return supplierDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }
        }


        public void UpdateContactPerson(SupplierDTO supplierDTO)
        {
            try
            {
                var contactPerson = context.ContactPersons.Find(supplierDTO.ContactId);

                if (contactPerson != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        contactPerson.FirstName = supplierDTO.ContactFirstName;

                        contactPerson.LastName = supplierDTO.ContactLastName;

                        contactPerson.Email = supplierDTO.ContactEmail;

                        contactPerson.PhoneNumber = supplierDTO.ContactPhoneNumber;

                        contactPerson.UpdatedDate = DateTime.Now;

                        contactPerson.UpdatedBy = supplierDTO.UpdatedBy;

                        contactPerson.CountryId = supplierDTO.ContactCountryId;

                        transaction.Commit();
                    }

                    context.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }



        public async Task<bool> CheckIfRecordExist(SupplierDTO supplierDTO)
        {
            try
            {
                bool result = await context.Suppliers.AnyAsync(p => p.PhoneNumber == supplierDTO.PhoneNumber & p.Email == supplierDTO.Email);

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
