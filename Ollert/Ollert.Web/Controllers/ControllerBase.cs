using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Web.Controllers
{
    /// <summary>
    /// Controllerekhez ősosztály
    /// </summary>
    public class ControllerBase : Controller
    {
        protected readonly ICategoryManager _categoryManager;

        public ControllerBase(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        protected void SetTitle(string title)
        {
            ViewBag.Title = title;
        }

        protected void FillViewBags()
        {
            ViewBag.Categories = _categoryManager.GetAll();
        }
    }
}