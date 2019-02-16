using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Repositories;
using System.Linq;

namespace Ollert.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly CardRepository _cardRepository;

        public CardController(CardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IActionResult Rename(int id, string cardName)
        {
            return Json(new
            {
                success = _cardRepository.Rename(id, cardName)
            });
        }

        public IActionResult SetDescription(int id, string description)
        {
            return Json(new
            {
                success = _cardRepository.SetDescription(id, description)
            });
        }

        public IActionResult AddNewComment(int id, string author, string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && text.Length > 0)
            {
                var commentId = _cardRepository.AddNewComment(id, author, text);
                var newComment = _cardRepository.Get(id).Comments.FirstOrDefault(x => x.Id == commentId);
                var date = newComment.CreationDate.ToLocalTime().ToString();
                var authorName = newComment.Author;
                return Json(new
                {
                    success = true,
                    date,
                    name = authorName
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}