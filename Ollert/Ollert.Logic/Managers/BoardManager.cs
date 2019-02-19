using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System.Linq;

namespace Ollert.Logic.Managers
{
    public class BoardManager : ManagerBase<Board, BoardDTO>, IBoardManager
    {
        public BoardManager(ISession session) : base(session)
        { }

        public bool RemoveCardList(int boardId, int cardListId)
        {
            var board = Get(boardId);
            var toDelete = board?.CardLists.FirstOrDefault(x => x.Id == cardListId);

            if (board == null || toDelete == null)
            {
                return false;
            }

            board.CardLists.Remove(toDelete);
            Save(board);

            return true;
        }

        public bool Rename(int id, string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name.Length > 0 && name.Length <= 255)
            {
                var board = Get(id);

                if (board == null)
                {
                    return false;
                }

                board.Name = name;
                Save(board);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
