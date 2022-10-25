using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.MedicalConditionModule;
using PharmCare.BLL.Repositories.ShelfModule;
using PharmCare.BLL.Repositories.SupplierModule;
using PharmCare.BLL.Repositories.UnitModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.MedicineModule;

namespace PharmCare.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class MedicinesController : Controller
    {
        private readonly IMedicineRepository medicineRepository;

        private readonly IUnitRepository unitRepository;

        private readonly IShelfRepository shelfRepository;

        private readonly ICategoryRepository categoryRepository;

        private readonly ISupplierRepository supplierRepository;

        private readonly UserManager<AppUser> userManager;

        private readonly IMedicalConditionRepository medicalConditionRepository;
        public MedicinesController(

            IMedicineRepository medicineRepository,

            UserManager<AppUser> userManager,

            IUnitRepository unitRepository,

            IShelfRepository shelfRepository,

            ICategoryRepository categoryRepository,

            ISupplierRepository supplierRepository,

            IMedicalConditionRepository medicalConditionRepository
            )
        {
            this.medicineRepository = medicineRepository;

            this.userManager = userManager;

            this.unitRepository = unitRepository;

            this.shelfRepository = shelfRepository;

            this.categoryRepository = categoryRepository;

            this.supplierRepository = supplierRepository;

            this.medicalConditionRepository = medicalConditionRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.MedicalConditions = await medicalConditionRepository.GetAll();

                ViewBag.Shelves = await shelfRepository.GetAll();

                ViewBag.Categories = await categoryRepository.GetAll();

                ViewBag.Manufacturers = await supplierRepository.GetAll();

                ViewBag.Units = await unitRepository.GetAll();

                var medecine = await medicineRepository.GetAll();

                return View(medecine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(MedicineDTO medicineDTO)
        {
            try
            {
            
                if (medicineDTO.CategoryId == null || medicineDTO.CategoryId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Select Category" });

                }        
               

                if (medicineDTO.ShelfId == null || medicineDTO.ShelfId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Select Medicine Location" });

                }

                var name = medicineDTO.Name.Substring(0, 1).ToUpper() + medicineDTO.Name.Substring(1).ToLower().Trim();

                bool IsPatientExist = (await medicineRepository.CheckIfRecordExist(medicineDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    medicineDTO.CreatedBy = user.Id;

                    medicineDTO.Name = name;

                    var result = await medicineRepository.Create(medicineDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Medicine has been successfully created" });
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
        public async Task<IActionResult> Update(MedicineDTO medicineDTO)
        {
            try
            {
               

                if (medicineDTO.CategoryId == null || medicineDTO.CategoryId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Select Category" });

                }


                if (medicineDTO.ShelfId == null || medicineDTO.ShelfId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Select Medicine Location" });

                }

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var name = medicineDTO.Name.Substring(0, 1).ToUpper() + medicineDTO.Name.Substring(1).ToLower().Trim();

                medicineDTO.UpdatedBy = user.Id;

                medicineDTO.Name = name;

                var results = await medicineRepository.Update(medicineDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Medicines details has been successfully updated" });
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
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await medicineRepository.Delete(Id);

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
        public async Task<IActionResult> GetExpenseTypes()
        {
            try
            {
                var expenseType = await medicineRepository.GetAll();

                return Json(new { data = expenseType });
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
                var medicine = await medicineRepository.GetById(Id);

                if (medicine != null)
                {
                    MedicineDTO file = new MedicineDTO()
                    {
                        Id = medicine.Id,

                        MedicalConditionId = medicine.MedicalConditionId,

                        Name = medicine.Name,

                        ShelfId = medicine.ShelfId,

                        ShelfName = medicine.ShelfName,

                        MedicalConditionName = medicine.MedicalConditionName,

                        Description = medicine.Description,

                        CategoryId = medicine.CategoryId,

                        CategoryName = medicine.CategoryName,

                        ManufacturerPrice = medicine.ManufacturerPrice,

                        SellingPrice = medicine.SellingPrice,

                        Status = medicine.Status,

                        CreateDate = medicine.CreateDate,

                        CreatedBy = medicine.CreatedBy,

                        UnitId = medicine.UnitId,

                        UnitName = medicine.UnitName,

                        Quantity = medicine.Quantity,

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
