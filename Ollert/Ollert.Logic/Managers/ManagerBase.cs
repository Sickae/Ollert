using AutoMapper;
using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ollert.Logic.Managers
{
    /// <summary>
    /// TEntity entitáshoz manager ősosztály
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ManagerBase<TEntity, TDto> : ManagerBase, IManagerBase<TEntity, TDto> where TEntity : Entity where TDto : DTOBase
    {
        public ManagerBase(ISession session) : base(session)
        { }

        public TDto Get(int id)
        {
            return Mapper.Map<TEntity, TDto>(_session.Get<TEntity>(id));
        }

        protected TEntity GetEntity(int id)
        {
            return _session.Get<TEntity>(id);
        }

        public virtual IList<TDto> GetAll()
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
                return 0;
            }

            entity.ModificationDate = DateTime.UtcNow;

            var cachedEntity = _session.Load<TEntity>(entity.Id);
            if (entity.Id == cachedEntity.Id)
            {
                _session.Evict(cachedEntity);
            }

            InTransaction(() =>
            {
                _session.SaveOrUpdate(entity);
            });

            return entity.Id;
        }
    }

    public abstract class ManagerBase
    {
        protected readonly ISession _session;

        public ManagerBase(ISession session)
        {
            _session = session;
        }

        protected void InTransaction(Action method)
        {
            using (var transaction = _session.BeginTransaction())
            {
                method();
                transaction.Commit();
            }
        }
    }
}
