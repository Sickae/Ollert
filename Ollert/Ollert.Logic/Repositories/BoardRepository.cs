using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Interfaces;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System;
using System.Linq;

namespace Ollert.Logic.Repositories
{
    public class BoardRepository : BoardManager, IUnitOfWorkRepository<BoardDTO>
    {
        private readonly CardListRepository _cardListRepository;

        public BoardRepository(CardListRepository cardListRepository, ISession session, IAppContext appContext)
            : base(session, appContext)
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

        public void Delete(BoardDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
