using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System.Collections.Generic;

namespace Ollert.Logic.Managers
{
    public class CategoryManager : ManagerBase<Category, CategoryDTO>, ICategoryManager
    {
        public CategoryManager(ISession session) : base(session)
        { }

        public int AddNewCategory(string categoryName)
        {
            if (!string.IsNullOrWhiteSpace(categoryName) && categoryName.Length > 0 && categoryName.Length <= 255)
            {
                var category = new CategoryDTO
                {
                    Name = categoryName,
                    Boards = new List<BoardDTO>()
                };

                return Save(category);
            }
            else
            {
                return 0;
            }
        }
    }
}
