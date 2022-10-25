using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;


namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        private readonly UserManager<AppUser> userManager;
        public CategoriesController(ICategoryRepository categoryRepository, UserManager<AppUser> userManager)
        {
            this.categoryRepository = categoryRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await categoryRepository.GetAll();

                return View(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(CategoryDTO categoryDTO)
        {
            try
            {
                var name = categoryDTO.Name.Substring(0, 1).ToUpper() + categoryDTO.Name.Substring(1).ToLower().Trim();

                bool IsPatientExist = (await categoryRepository.CheckIfRecordExist(categoryDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    categoryDTO.CreatedBy = user.Id;

                    categoryDTO.Name = name;

                    var result = await categoryRepository.Create(categoryDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Category has been successfully created" });
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
        public async Task<IActionResult> Update(CategoryDTO categoryDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var name = categoryDTO.Name.Substring(0, 1).ToUpper() + categoryDTO.Name.Substring(1).ToLower().Trim();

                categoryDTO.UpdatedBy = user.Id;

                categoryDTO.Name = name;    

                var results = await categoryRepository.Update(categoryDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Category details has been successfully updated" });
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
                var results = await categoryRepository.Delete(Id);

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
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var expenseType = await categoryRepository.GetById(Id);

                if (expenseType != null)
                {
                    CategoryDTO file = new CategoryDTO()
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
