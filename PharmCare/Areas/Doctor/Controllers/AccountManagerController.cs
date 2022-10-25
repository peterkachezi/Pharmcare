using Microsoft.AspNetCore.Mvc;

namespace PharmCare.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class AccountManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
