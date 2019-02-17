using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;

namespace Ollert.Logic.Repositories
{
    public class CategoryRepository : CategoryManager, IUnitOfWorkRepository<CategoryDTO>
    {
        public CategoryRepository(ISession session) : base(session)
        { }
    }
}
