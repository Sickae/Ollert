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
            var id = _cardListRepository.AddNewCard(cardListId, cardName);

            if (id > 0)
            {
                return Json(new
                {
                    success = true,
                    id
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveCard(int cardListId, int cardId)
        {
            if (_cardListRepository.RemoveCard(cardListId, cardId))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveAllCards(int id)
        {
            if (_cardListRepository.RemoveAllCards(id))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult Rename(int id, string cardListName)
        {
            return Json(new
            {
                success = _cardListRepository.Rename(id, cardListName)
            });
        }
    }
}