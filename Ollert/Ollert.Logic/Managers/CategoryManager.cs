using AutoMapper;
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

        public override IList<CategoryDTO> GetAll()
        {
            var entities = _session.QueryOver<Category>().Where(x => !x.IsDeleted).List();
            return Mapper.Map<IList<Category>, IList<CategoryDTO>>(entities);
        }

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

        public bool RemoveCategory(int id)
        {
            var category = GetEntity(id);

            if (category == null)
            {
                return false;
            }

            category.IsDeleted = true;
            Save(category);

            return true;
        }

        public bool Rename(int id, string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name.Length > 0 && name.Length <= 255)
            {
                var category = Get(id);

                if (category == null)
                {
                    return false;
                }

                category.Name = name;
                Save(category);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
