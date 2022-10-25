using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.ShelfModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;

using PharmCare.DTO.ShelfModule;

namespace PharmCare.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class ShelvesController : Controller
    {
        private readonly IShelfRepository shelfRepository;

        private readonly UserManager<AppUser> userManager;
        public ShelvesController(IShelfRepository shelfRepository, UserManager<AppUser> userManager)
        {
            this.shelfRepository = shelfRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var locations = await shelfRepository.GetAll();

                return View(locations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(ShelfDTO shelfDTO)
        {
            try
            {
                var name = shelfDTO.Name.ToUpper();

                bool IsPatientExist = (await shelfRepository.CheckIfRecordExist(shelfDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    shelfDTO.CreatedBy = user.Id;

                    shelfDTO.Name = name;

                    var result = await shelfRepository.Create(shelfDTO);

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
        public async Task<IActionResult> Update(ShelfDTO shelfDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var name = shelfDTO.Name.ToUpper();

                shelfDTO.UpdatedBy = user.Id;

                shelfDTO.Name = name;

                var results = await shelfRepository.Update(shelfDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "New details has been successfully updated" });
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
                var results = await shelfRepository.Delete(Id);

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
                var expenseType = await shelfRepository.GetAll();

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
                var expenseType = await shelfRepository.GetById(Id);

                if (expenseType != null)
                {
                    ShelfDTO file = new ShelfDTO()
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
