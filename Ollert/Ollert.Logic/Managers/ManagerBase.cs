using AutoMapper;
using NHibernate;
using NHibernate.Exceptions;
using Npgsql;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Helpers;
using Ollert.Logic.Interfaces;
using Ollert.Logic.Managers.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ollert.Logic.Managers
{
    /// <summary>
    /// TEntity entitáshoz manager ősosztály
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ManagerBase<TEntity, TDto> : ManagerBase, IManagerBase<TEntity, TDto> where TEntity : Entity where TDto : DTOBase
    {
        public ManagerBase(ISession session, IAppContext appContext) : base(session, appContext)
        { }

        public void Delete(int id)
        {
            try
            {
                InTransaction(() =>
                {
                    var entity = _session.Get<TEntity>(id);
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
                        var entity = _session.Get<TEntity>(id);
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

        public TDto Get(int id)
        {
            return Mapper.Map<TEntity, TDto>(_session.Get<TEntity>(id));
        }

        public IList<TDto> Get(IList<int> ids)
        {
            if (ids == null)
            {
                Log.Logger.Error("Null id list during Delete.");
                return new List<TDto>();
            }

            var list = ids.Select(Get).ToList();
            list.RemoveAll(x => x == null);

            return list;
        }

        public IList<TDto> GetAll()
        {
            var entities = _session.QueryOver<TEntity>().List();
            var dtos = Mapper.Map<IList<TEntity>, IList<TDto>>(entities);
            return dtos;
        }

        public virtual int Save(TDto dto)
        {
            var entity = Mapper.Map<TDto, TEntity>(dto);
            return Save(entity);
        }

        protected int Save(TEntity entity)
        {
            if (entity == null)
            {
                Log.Logger.Error("Null entity during Save");
                return 0;
            }

            entity.ModificationDate = DateTime.UtcNow;

            var cachedEntity = _session.Load<TEntity>(entity.Id);
            if (entity.Id == cachedEntity.Id)
            {
                _session.Evict(cachedEntity);
            }

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

        protected virtual void OnDeleting(TEntity entity)
        {

        }
    }

    public abstract class ManagerBase
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
