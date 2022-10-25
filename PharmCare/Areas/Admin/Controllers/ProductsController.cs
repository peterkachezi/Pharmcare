using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.LeafSettingModule;
using PharmCare.BLL.Repositories.ProductModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.LeafSettingModule;
using PharmCare.DTO.ProductModule;

namespace PharmCare.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly UserManager<AppUser> userManager;
        public ProductsController(IProductRepository productRepository, UserManager<AppUser> userManager)
        {
            this.productRepository = productRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var leafSettings = await productRepository.GetAll();

                return View(leafSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            try
            {

                bool IsPatientExist = (await productRepository.CheckIfRecordExist(productDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    productDTO.CreatedBy = user.Id;

                    var result = await productRepository.Create(productDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Record has been successfully created" });
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
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                productDTO.UpdatedBy = user.Id;

                var results = await productRepository.Update(productDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Record details has been successfully updated" });
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
                var results = await productRepository.Delete(Id);

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
                var expenseType = await productRepository.GetAll();

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
                var expenseType = await productRepository.GetById(Id);

                if (expenseType != null)
                {
                    ProductDTO file = new ProductDTO()
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
