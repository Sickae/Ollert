using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Repositories;
using Ollert.Web.Models;

namespace Ollert.Web.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult CategoryList()
        {
            var categories = _categoryRepository.GetAll();
            var vm = new CategoryListViewModel { Categories = categories };
            SetTitle("Táblák");
            return View(vm);
        }

        public IActionResult AddNewCategory(string categoryName)
        {
            var id = _categoryRepository.AddNewCategory(categoryName);

            if (id > 0)
            {
                return Json(new
                {
                    success = true,
                    id
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveCategory(int id)
        {
            return Json(new
            {
                success = _categoryRepository.RemoveCategory(id)
            });
        }

        public IActionResult Rename(int id, string name)
        {
            return Json(new
            {
                success = _categoryRepository.Rename(id, name)
            });
        }
    }
}