using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Interfaces;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class BoardManager : ManagerBase<Board, BoardDTO>, IBoardManager
    {
        public BoardManager(ISession session, IAppContext appContext) : base(session, appContext)
        { }
    }
}
