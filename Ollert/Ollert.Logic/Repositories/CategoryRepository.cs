using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System;

namespace Ollert.Logic.Repositories
{
    public class CategoryRepository : CategoryManager, IUnitOfWorkRepository<CategoryDTO>
    {
        public CategoryRepository(ISession session) : base(session)
        { }

        public void Delete(CategoryDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
