using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Web.Models;

namespace Ollert.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(ICategoryManager categoryManager) : base(categoryManager)
        { }

        public IActionResult Error(int statusCode)
        {
            var model = new ErrorViewModel
            {
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 404:
                {
                    model.Message = "Nem található";
                    break;
                }
                default:
                {
                    model.Message = "Ismeretlen hiba";
                    break;
                }
            }

            SetTitle("Hiba");
            FillViewBags();
            ViewBag.HideNavTitle = true;

            return View(nameof(Error), model);
        }
    }
}