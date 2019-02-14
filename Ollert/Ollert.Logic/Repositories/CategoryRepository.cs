using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System.Collections.Generic;

namespace Ollert.Logic.Repositories
{
    public class CategoryRepository : CategoryManager, IUnitOfWorkRepository<CategoryDTO>
    {
        public CategoryRepository(ISession session) : base(session)
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
