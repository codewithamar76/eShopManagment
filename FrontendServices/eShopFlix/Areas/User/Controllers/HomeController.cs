using Microsoft.AspNetCore.Mvc;

namespace eShopFlix.Areas.User.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
