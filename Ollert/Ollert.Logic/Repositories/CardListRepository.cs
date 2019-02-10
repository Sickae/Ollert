using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System;
using System.Linq;

namespace Ollert.Logic.Repositories
{
    public class CardListRepository : CardListManager, IUnitOfWorkRepository<CardListDTO>
    {
        private readonly CardRepository _cardRepository;

        public CardListRepository(CardRepository cardRepository, ISession session) : base(session)
        {
            _cardRepository = cardRepository;
        }

        public override int Save(CardListDTO dto)
        {
            var unsavedCards = dto.Cards.Where(x => x.Id == 0).ToList();
            dto.Cards.ToList().RemoveAll(x => x.Id == 0);

            foreach (var card in unsavedCards)
            {
                card.Id = _cardRepository.Save(card);
            }

            dto.Cards.ToList().AddRange(unsavedCards);

            return base.Save(dto);
        }

        public void Delete(CardListDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
