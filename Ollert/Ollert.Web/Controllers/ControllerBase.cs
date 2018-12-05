using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Web.Controllers
{
    /// <summary>
    /// Controllerekhez ősosztály
    /// </summary>
    public class ControllerBase : Controller
    {
        private readonly IManagerBase _manager;

        protected ControllerBase(IManagerBase manager)
        {
            _manager = manager;
        }

        protected void SetTitle(string title)
        {
            ViewBag.Title = title;
        }
    }
}