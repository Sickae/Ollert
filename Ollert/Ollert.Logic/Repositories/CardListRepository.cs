using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System.Collections.Generic;
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

        public int AddNewCard(int cardListId, string cardName)
        {
            if (!string.IsNullOrWhiteSpace(cardName) && cardName.Length > 0 && cardName.Length <= 255)
            {
                var card = new CardDTO
                {
                    Name = cardName,
                    Description = "",
                    Comments = new List<CommentDTO>(),
                    Labels = new List<LabelDTO>()
                };

                var cardList = Get(cardListId);

                if (cardList == null)
                {
                    return 0;
                }

                cardList.Cards.Add(card);
                Save(cardList);
                return card.Id;
            }
            else
            {
                return 0;
            }
        }

    }
}
