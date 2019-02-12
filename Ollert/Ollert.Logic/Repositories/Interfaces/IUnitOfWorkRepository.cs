using Ollert.Logic.DTOs;

namespace Ollert.Logic.Repositories.Interfaces
{
    public interface IUnitOfWorkRepository<TDto> : IUnitOfWorkRepository where TDto : DTOBase
    {
        int Save(TDto dto);
    }

    public interface IUnitOfWorkRepository { }
}
