using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CountryModule;
using PharmCare.BLL.Repositories.ProductTypeModule;
using PharmCare.BLL.Repositories.SupplierModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.SupplierModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuppliersController : Controller
    {
        private readonly ICountryRepository countryRepository;

        private readonly ISupplierRepository  supplierRepository;

        private readonly IProductTypeRepository productTypeRepository;

        private readonly UserManager<AppUser> userManager;
        public SuppliersController(IProductTypeRepository productTypeRepository, ISupplierRepository supplierRepository, ICountryRepository countryRepository, UserManager<AppUser> userManager)
        {
            this.countryRepository = countryRepository;

            this.userManager = userManager;

            this.supplierRepository = supplierRepository;

            this.productTypeRepository = productTypeRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Countries = await countryRepository.GetAll();

                ViewBag.ProductTypes = await productTypeRepository.GetAll();

                var suppliers = await supplierRepository.GetAll();

                return View(suppliers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });

            }
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(SupplierDTO supplierDTO)
        {
            try
            {
                //if (supplierDTO.CountryId == null || supplierDTO.CountryId == Guid.Empty)
                //{
                //    return Json(new { success = false, responseText = "Please select supplier's  town" });

                //}                              

                //if (supplierDTO.ProductTypeId == null || supplierDTO.ProductTypeId == Guid.Empty)
                //{
                //    return Json(new { success = false, responseText = "Please select supplier's product type" });

                //}

                var firstName = supplierDTO.Name.Substring(0, 1).ToUpper() + supplierDTO.Name.Substring(1).ToLower().Trim();

                var email = supplierDTO.Email.ToLower().Trim();

                bool IsPatientExist = (await supplierRepository.CheckIfRecordExist(supplierDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    supplierDTO.CreatedBy = user.Id;

                    supplierDTO.Name = firstName;

                    supplierDTO.Email = email;

                    var result = await supplierRepository.Create(supplierDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Supplier has been successfully created" });
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Failed to create record" });
                    }
                }

                if (IsPatientExist == true)
                {
                    return Json(new { success = false, responseText = " A record with the same details already exists" });
                }

                else
                {
                    return Json(new { success = false, responseText = "Something went wrong" });

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await supplierRepository.Delete(Id);

                if (results == true)
                {
                    return Json(new { success = true, responseText = "Record  has been successfully deleted " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Record has not been deleted ,it could be in use by other files" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> Update(SupplierDTO supplierDTO)
        {
            try
            {

                if (supplierDTO.CountryId == null || supplierDTO.CountryId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Please select manufucturer's  country" });

                }              

                if (supplierDTO.ProductTypeId == null || supplierDTO.ProductTypeId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Please select product type" });

                }

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                supplierDTO.UpdatedBy = user.Id;

                var name = supplierDTO.Name.Substring(0, 1).ToUpper() + supplierDTO.Name.Substring(1).ToLower().Trim();

                var email = supplierDTO.Email.ToLower().Trim();

                supplierDTO.Name = name;

                supplierDTO.Email = email;

                //var IsRecordExist = (await supplierRepository.CheckIfRecordExist(supplierDTO));

                //if (IsRecordExist == true)
                //{
                //    return Json(new { success = true, responseText = "Details has been successfully updated" });
                //}

                var results = await supplierRepository.Update(supplierDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Details has been successfully updated" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to update record!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }

        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var supplier = await supplierRepository.GetById(Id);

                if (supplier != null)
                {
                    SupplierDTO file = new SupplierDTO()
                    {
                        Id = supplier.Id,

                        Name = supplier.Name,

                        CountryId = supplier.CountryId,

                        ProductTypeId = supplier.ProductTypeId,

                        SupplierNo = supplier.SupplierNo,

                        PhoneNumber = supplier.PhoneNumber,

                        Email = supplier.Email,

                        Town = supplier.Town,

                        PhysicalAddress = supplier.PhysicalAddress,

                        CreateDate = supplier.CreateDate,

                        CreatedBy = supplier.CreatedBy,

                        ContactId = supplier.ContactId,

                        ContactFirstName = supplier.ContactFirstName,

                        ContactLastName = supplier.ContactLastName,

                        ContactEmail = supplier.ContactEmail,

                        ContactPhoneNumber = supplier.ContactPhoneNumber,

                        ContactCountryId = supplier.ContactCountryId,
                    };

                    return Json(new { data = file });
                }

                return Json(new { data = false });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }



    }
}
