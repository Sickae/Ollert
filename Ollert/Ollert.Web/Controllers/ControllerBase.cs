using Microsoft.AspNetCore.Mvc;

namespace Ollert.Web.Controllers
{
    /// <summary>
    /// Controllerekhez ősosztály
    /// </summary>
    public class ControllerBase : Controller
    {
        protected void SetTitle(string title)
        {
            ViewBag.Title = title;
        }
    }
}