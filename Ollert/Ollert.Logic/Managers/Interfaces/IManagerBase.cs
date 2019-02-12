using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using System.Collections.Generic;

namespace Ollert.Logic.Managers.Interfaces
{
    public interface IManagerBase<T, TDto> : IManagerBase where T : Entity where TDto : DTOBase
    {
        TDto Get(int id);
        IList<TDto> GetAll();
        int Save(TDto dto);
    }

    public interface IManagerBase { }
}
