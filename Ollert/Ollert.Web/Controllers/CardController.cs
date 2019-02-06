using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Repositories;

namespace Ollert.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly CardRepository _cardRepository;

        public CardController(CardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IActionResult EditCardName(int id, string cardName)
        {
            if (!string.IsNullOrWhiteSpace(cardName) && cardName.Length > 0 && cardName.Length <= 255)
            {
                var card = _cardRepository.Get(id);
                card.Name = cardName;
                _cardRepository.Save(card);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}