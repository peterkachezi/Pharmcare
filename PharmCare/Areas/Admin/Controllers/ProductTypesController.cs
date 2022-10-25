using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.ProductTypeModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.ProductTypeModule;
using System.Xml.Linq;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly IProductTypeRepository productTypeRepository;

        private readonly UserManager<AppUser> userManager;
        public ProductTypesController(IProductTypeRepository productTypeRepository, UserManager<AppUser> userManager)
        {
            this.productTypeRepository = productTypeRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await productTypeRepository.GetAll();

                return View(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(ProductTypeDTO productTypeDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                productTypeDTO.CreatedBy = user.Id;

                var result = await productTypeRepository.Create(productTypeDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Product Type has been successfully created" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to create record" });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> Update(ProductTypeDTO productTypeDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);              

                productTypeDTO.UpdatedBy = user.Id;

                var results = await productTypeRepository.Update(productTypeDTO);

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
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await productTypeRepository.Delete(Id);

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
                var expenseType = await productTypeRepository.GetAll();

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
                var expenseType = await productTypeRepository.GetById(Id);

                if (expenseType != null)
                {
                    ProductTypeDTO file = new ProductTypeDTO()
                    {
                        Id = expenseType.Id,

                        Name = expenseType.Name,

                        CreateDate = expenseType.CreateDate,
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
