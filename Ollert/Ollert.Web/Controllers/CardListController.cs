using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.DTOs;
using Ollert.Logic.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Ollert.Web.Controllers
{
    public class CardListController : Controller
    {
        private readonly CardListRepository _cardListRepository;

        public CardListController(CardListRepository cardListRepository)
        {
            _cardListRepository = cardListRepository;
        }

        public IActionResult AddNewCard(int cardListId, string cardName)
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

                var cardList = _cardListRepository.Get(cardListId);
                cardList.Cards.Add(card);
                _cardListRepository.Save(cardList);

                return Json(new
                {
                    success = true,
                    id = card.Id
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveCard(int cardListId, int cardId)
        {
            var cardList = _cardListRepository.Get(cardListId);
            var card = cardList.Cards.FirstOrDefault(x => x.Id == cardId);

            cardList.Cards.Remove(card);
            _cardListRepository.Save(cardList);

            return Json(new { success = true });
        }

        public IActionResult Rename(int id, string cardListName)
        {
            if (!string.IsNullOrEmpty(cardListName) && cardListName.Length > 0 && cardListName.Length <= 255)
            {
                var cardList = _cardListRepository.Get(id);
                cardList.Name = cardListName;
                _cardListRepository.Save(cardList);

                return Json(new
                {
                    success = true,
                    id = cardList.Id,
                    name = cardList.Name
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}