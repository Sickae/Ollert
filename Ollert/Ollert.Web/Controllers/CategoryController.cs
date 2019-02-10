using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.DTOs;
using Ollert.Logic.Repositories;
using Ollert.Web.Models;
using System.Collections.Generic;

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
            if (!string.IsNullOrWhiteSpace(categoryName) && categoryName.Length > 0 &&categoryName.Length <= 255)
            {
                var category = new CategoryDTO
                {
                    Name = categoryName,
                    Boards = new List<BoardDTO>()
                };
                _categoryRepository.Save(category);

                return Json(new
                {
                    success = true,
                    category
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}