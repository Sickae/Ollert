using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using System.Linq;

namespace Ollert.Logic.Managers
{
    public class CardManager : ManagerBase<Card, CardDTO>, ICardManager
    {
        public CardManager(ISession session) : base(session)
        { }

        public bool Rename(int id, string cardName)
        {
            if (!string.IsNullOrWhiteSpace(cardName) && cardName.Length > 0 && cardName.Length <= 255)
            {
                var card = Get(id);

                if (card == null)
                {
                    return false;
                }

                card.Name = cardName;
                Save(card);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetDescription(int id, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                var card = Get(id);

                if (card == null)
                {
                    return false;
                }

                card.Description = description;
                Save(card);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveComment(int cardId, int commentId)
        {
            var card = Get(cardId);
            var toDelete = card?.Comments.FirstOrDefault(x => x.Id == commentId);

            if (card == null || toDelete == null)
            {
                return false;
            }

            card.Comments.Remove(toDelete);
            Save(card);

            return true;
        }
    }
}
