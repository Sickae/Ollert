using Ollert.DataAccess.Entitites;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ollert.Logic.Managers.Interfaces
{
    public interface IManagerBase<T> : IManagerBase where T : Entity
    {
        T Get(int id);
        IList<T> Get(IList<int> ids);
        IList<T> GetAll();
        IList<T> GetAll(Expression<Func<T, bool>> expression);
        int Save(T entity);
        void Delete(int id);
        void Delete(IList<int> ids);
    }

    public interface IManagerBase { }
}
