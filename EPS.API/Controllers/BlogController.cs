using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
