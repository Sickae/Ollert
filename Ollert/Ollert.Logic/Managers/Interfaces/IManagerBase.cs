using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ollert.Logic.Managers.Interfaces
{
    public interface IManagerBase<T, TDto> : IManagerBase where T : Entity where TDto : DTOBase
    {
        TDto Get(int id);
        IList<TDto> Get(IList<int> ids);
        IList<TDto> GetAll();
        IList<TDto> GetAll(Expression<Func<TDto, bool>> expression);
        int Save(TDto dto);
        void Delete(int id);
        void Delete(IList<int> ids);
    }

    public interface IManagerBase { }
}
