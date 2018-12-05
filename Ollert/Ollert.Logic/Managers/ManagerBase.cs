using Ollert.DataAccess.Entitites;
using Ollert.Logic.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ollert.Logic.Managers
{
    /// <summary>
    /// T entitáshoz manager ősosztály
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ManagerBase<T> : IManagerBase<T> where T : Entity
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<T> Get(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public int Save(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
