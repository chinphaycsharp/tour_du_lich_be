using EPS.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/contact")]
    [Authorize]
    public class ContactController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
