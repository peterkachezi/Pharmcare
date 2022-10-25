using Microsoft.AspNetCore.Mvc;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReturnsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
