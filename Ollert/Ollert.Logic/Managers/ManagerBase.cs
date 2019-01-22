using NHibernate;
using NHibernate.Exceptions;
using Npgsql;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.Helpers;
using Ollert.Logic.Interfaces;
using Ollert.Logic.Managers.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Ollert.Logic.Managers
{
    /// <summary>
    /// T entitáshoz manager ősosztály
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ManagerBase<T> : ManagerBase, IManagerBase<T> where T : Entity
    {
        public ManagerBase(ISession session, IAppContext appContext) : base(session, appContext)
        { }

        public void Delete(int id)
        {
            try
            {
                InTransaction(() =>
                {
                    var entity = _session.Get<T>(id);
                    if (entity == null)
                    {
                        Log.Logger.Error($"Null entity with id {id} during Delete.");
                        return;
                    }

                    OnDeleting(entity);
                    _session.Delete(entity);
                });
            }
            catch (GenericADOException ex)
            {
                Log.Logger.Error(ex, "Commit error during Delete.");

                if (ex.InnerException is PostgresException pgex)
                {
                    if (pgex.SqlState == Constants.ErrorCodes.SqlUniqueKeyViolation)
                    {
                        throw new ConstraintException("Foreign key rule violated.", pgex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Delete(IList<int> ids)
        {
            if (ids == null)
            {
                Log.Logger.Error("Null id list during Delete.");
                return;
            }

            try
            {
                InTransaction(() =>
                {
                    foreach (var id in ids)
                    {
                        var entity = _session.Get<T>(id);
                        if (entity == null)
                        {
                            Log.Logger.Error($"Null entity with id {id} during Delete.");
                            // TODO jó ötlet megszakítani az egész törlést? nem lesz rollback
                            return;
                        }

                        OnDeleting(entity);
                        _session.Delete(entity);
                    }
                });
            }
            catch (GenericADOException ex)
            {
                Log.Logger.Error(ex, "Commit error during Delete.");

                if (ex.InnerException is PostgresException pgex)
                {
                    if (pgex.SqlState == Constants.ErrorCodes.SqlUniqueKeyViolation)
                    {
                        throw new ConstraintException("Foreign key rule violated.", pgex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public T Get(int id)
        {
            return _session.Get<T>(id);
        }

        public IList<T> Get(IList<int> ids)
        {
            if (ids == null)
            {
                Log.Logger.Error("Null id list during Delete.");
                return new List<T>();
            }

            var list = ids.Select(Get).ToList();
            list.RemoveAll(x => x == null);

            return list;
        }

        public IList<T> GetAll()
        {
            return _session.QueryOver<T>().List();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                Log.Logger.Error("Null expression during GetAll");
                return new List<T>();
            }

            return _session.QueryOver<T>().Where(expression).List();
        }

        public int Save(T entity)
        {
            if (entity == null)
            {
                Log.Logger.Error("Null entity during Save");
                return 0;
            }

            entity.ModificationDate = DateTime.UtcNow;

            try
            {
                InTransaction(() =>
                {
                    _session.SaveOrUpdate(entity);
                });
                return entity.Id;
            }
            catch (GenericADOException ex)
            {
                Log.Logger.Error(ex, "Commit error during Save.");
                throw;
            }
        }

        protected virtual void OnDeleting(T entity)
        {

        }
    }

    public abstract class ManagerBase : IManagerBase
    {
        protected readonly ISession _session;
        protected readonly IAppContext _appContext;

        public ManagerBase(ISession session, IAppContext appContext)
        {
            _session = session;
            _appContext = appContext;
        }

        protected void InTransaction(Action method)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    method();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Commit error.");
                    transaction.Rollback();
                    _session.Clear();
                    throw;
                }
            }
        }
    }
}
