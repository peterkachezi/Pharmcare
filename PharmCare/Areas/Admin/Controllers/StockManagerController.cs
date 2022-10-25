using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.StockModule;
using PharmCare.BLL.Repositories.SupplierModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.LeafSettingModule;
using PharmCare.DTO.MedicineModule;
using PharmCare.DTO.StockModule;
using PharmCare.DTO.SupplierModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockManagerController : Controller
    {
        private readonly ISupplierRepository supplierRepository;

        private readonly IMedicineRepository medicineRepository;

        private readonly IStockRepository stockRepository;

        private readonly UserManager<AppUser> userManager;
        public StockManagerController(IStockRepository stockRepository, UserManager<AppUser> userManager, ISupplierRepository supplierRepository, IMedicineRepository medicineRepository)
        {
            this.supplierRepository = supplierRepository;

            this.medicineRepository = medicineRepository;

            this.userManager = userManager;

            this.stockRepository = stockRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {

                ViewBag.Manufacturers = await supplierRepository.GetAll();

                ViewBag.Medicines = await medicineRepository.GetAll();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> OutOfStockProducts()
        {
            try
            {
                var outOfStockProducts = (await medicineRepository.GetAll()).Where(x => x.Quantity == 0);

                return View(outOfStockProducts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> CreateStock(GoodsReceivedHistoryDTO stockDetailDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                stockDetailDTO.CreatedBy = user.Id;

                var result = await stockRepository.CreateSingleEntry(stockDetailDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Stock has been successfully updated" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to update stock" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> ViewStock()
        {
            try
            {
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
        public async Task<IActionResult> GetByMedicineId(Guid Id)
        {
            try
            {

                var medicine = await medicineRepository.GetById(Id);

                var prescriptionId = Id;

                if (medicine != null)
                {
                    MedicineDTO file = new MedicineDTO()
                    {
                        Id = medicine.Id,

                        MedicalConditionId = medicine.MedicalConditionId,

                        Name = medicine.Name,

                        ShelfId = medicine.ShelfId,

                        ShelfName = medicine.ShelfName,

                        Description = medicine.Description,

                        CategoryId = medicine.CategoryId,

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

        [HttpPost]
        public async Task<ActionResult> SaveTransaction(GoodsReceivedNoteDTO goodsReceivedNoteDTO)
        {
            try
            {

                if (goodsReceivedNoteDTO.SupplierId == null || goodsReceivedNoteDTO.SupplierId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Please select supplier" });
                }

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                goodsReceivedNoteDTO.CreatedBy = user.Id;

                goodsReceivedNoteDTO.Id = Guid.NewGuid();

                var result = await stockRepository.SaveStock(goodsReceivedNoteDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = goodsReceivedNoteDTO.GRNo });
                }

                else
                {
                    return Json(new { success = false, responseText = "Transaction was not successfull" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public IActionResult ExpiredProducts()
        {
            try
            {
                var medecine = stockRepository.GetExpiredProducts();

                return View(medecine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
    }
}
