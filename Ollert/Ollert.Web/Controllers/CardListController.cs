using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.DTOs;
using Ollert.Logic.Repositories;
using System.Collections.Generic;

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
            var card = new CardDTO
            {
                Name = cardName,
                Description = "",
                Comments = new List<CommentDTO>(),
                Labels = new List<LabelDTO>()
            };

            var cardList = _cardListRepository.Get(cardListId);
            cardList.Cards.Add(card);
            var id = _cardListRepository.Save(cardList);

            return Json(new
            {
                success = true,
                id
            });
        }
    }
}