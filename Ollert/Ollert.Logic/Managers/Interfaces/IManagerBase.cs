using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using System.Collections.Generic;

namespace Ollert.Logic.Managers.Interfaces
{
    public interface IManagerBase<T, TDto> where T : Entity where TDto : DTOBase
    {
        TDto Get(int id);
        IList<TDto> Get(IList<int> ids);
        IList<TDto> GetAll();
        int Save(TDto dto);
        void Delete(int id);
        void Delete(IList<int> ids);
    }
}
