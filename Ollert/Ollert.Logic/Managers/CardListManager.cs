using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Ollert.Logic.Managers
{
    public class CardListManager : ManagerBase<CardList, CardListDTO>, ICardListManager
    {
        public CardListManager(ISession session) : base(session)
        { }

        public bool RemoveAllCards(int id)
        {
            var list = Get(id);

            if (list == null)
            {
                return false;
            }

            list.Cards = new List<CardDTO>();
            Save(list);

            return true;
        }

        public bool RemoveCard(int cardListId, int cardId)
        {
            var cardList = Get(cardListId);
            var toDelete = cardList?.Cards.FirstOrDefault(x => x.Id == cardId);

            if (cardList == null || toDelete == null)
            {
                return false;
            }

            cardList.Cards.Remove(toDelete);
            Save(cardList);

            return true;
        }

        public bool Rename(int id, string cardListName)
        {
            if (!string.IsNullOrEmpty(cardListName) && cardListName.Length > 0 && cardListName.Length <= 255)
            {
                var cardList = Get(id);

                if (cardList == null)
                {
                    return false;
                }

                cardList.Name = cardListName;
                Save(cardList);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
