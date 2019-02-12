using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System.Collections.Generic;

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
    }
}
