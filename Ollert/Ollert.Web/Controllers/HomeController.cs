using Microsoft.AspNetCore.Mvc;

namespace Ollert.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}