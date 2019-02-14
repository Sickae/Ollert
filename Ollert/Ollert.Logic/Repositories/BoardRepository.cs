using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Ollert.Logic.Repositories
{
    public class BoardRepository : BoardManager, IUnitOfWorkRepository<BoardDTO>
    {
        private readonly CardListRepository _cardListRepository;

        public BoardRepository(CardListRepository cardListRepository, ISession session) : base(session)
        {
            _cardListRepository = cardListRepository;
        }

        public override int Save(BoardDTO dto)
        {
            var unsavedCardLists = dto.CardLists.Where(x => x.Id == 0).ToList();
            dto.CardLists.ToList().RemoveAll(x => x.Id == 0);

            foreach (var cardList in unsavedCardLists)
            {
                cardList.Id = _cardListRepository.Save(cardList);
            }

            dto.CardLists.ToList().AddRange(unsavedCardLists);

            return base.Save(dto);
        }

        public int AddNewCardList(int boardId, string cardListName)
        {
            if (!string.IsNullOrWhiteSpace(cardListName) && cardListName.Length > 0 && cardListName.Length <= 255)
            {
                var cardList = new CardListDTO
                {
                    Name = cardListName,
                    Cards = new List<CardDTO>()
                };

                var board = Get(boardId);

                if (board == null)
                {
                    return 0;
                }

                board.CardLists.Add(cardList);
                return Save(board);
            }

            return 0;
        }
    }
}
