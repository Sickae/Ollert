using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Logic.Repositories;

namespace Ollert.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly CardRepository _cardRepository;
        private readonly ICommentManager _commentManager;

        public CardController(CardRepository cardRepository, ICommentManager commentManager)
        {
            _cardRepository = cardRepository;
            _commentManager = commentManager;
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
                var newComment = _commentManager.Get(commentId);
                var date = newComment.CreationDate.ToLocalTime().ToString();
                var authorName = newComment.Author;
                return Json(new
                {
                    success = true,
                    id = commentId,
                    date,
                    name = authorName
                });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveComment(int cardId, int commentId)
        {
            if (_cardRepository.RemoveComment(cardId, commentId))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}