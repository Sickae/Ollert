using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class CategoryManager : ManagerBase<Category, CategoryDTO>, ICategoryManager
    {
        public CategoryManager(ISession session) : base(session)
        { }
    }
}
