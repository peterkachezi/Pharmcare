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
using static SkiaSharp.HarfBuzz.SKShaper;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockManagerController : Controller
    {
        private readonly ISupplierRepository supplierRepository;

        private readonly IMedicineRepository medicineRepository;

        private readonly IStockRepository stockRepository;

        private readonly UserManager<AppUser> userManager;
        public StockManagerController(

            IStockRepository stockRepository,

            UserManager<AppUser> userManager,

            ISupplierRepository supplierRepository,

            IMedicineRepository medicineRepository)
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
        public async Task<IActionResult> UpdateStock(GoodsReceivedHistoryDTO stockDetailDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                stockDetailDTO.UpdatedBy = user.Id;

                var result = await stockRepository.UpdateStock(stockDetailDTO);

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
        public async Task<IActionResult> DeleteFromStock(Guid Id)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var result = await stockRepository.DeleteFromStock(Id);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Stock has been successfully deleted" });
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
                var stock = (await medicineRepository.GetAll()).Where(x => x.Status != 2).ToList();

                return View(stock);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public IActionResult StockEntryHistory()
        {
            try
            {
                var history = stockRepository.GetStockEntryHistory();

                return View(history);
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

                var stock = await medicineRepository.GetByStockId(Id);

                var prescriptionId = Id;

                if (stock != null)
                {
                    return Json(new { data = stock });
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
        public IActionResult ExpiredDrugs()
        {
            try
            {
                var medecine = stockRepository.GetExpiredProducts().Where(x => x.Status != 2);

                return View(medecine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public IActionResult DeleteExpiredDrugs(string BatchNo)
        {
            try
            {
                var result = stockRepository.DeleteExpiredDrugs(BatchNo);

                if (result == true)
                {
                    return Json(new { success = true, responseText = "Record has been deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to delete this record " });
                }
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
