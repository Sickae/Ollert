using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ollert.DataAccess.Enums;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Web.Models;
using System.Collections.Generic;

namespace Ollert.Web.Controllers
{
    public class BoardController : ControllerBase
    {
        private readonly IBoardManager _boardManager;
        private readonly ICardListManager _cardListManager;
        private readonly ICardManager _cardManager;
        private readonly ILabelManager _labelManager;
        private readonly ICommentManager _commentManager;

        public BoardController(IBoardManager boardManager, ICardListManager cardListManager, ICardManager cardManager,
            ILabelManager labelManager, ICommentManager commentManager) : base(boardManager)
        {
            _boardManager = boardManager;
            _cardListManager = cardListManager;
            _cardManager = cardManager;
            _labelManager = labelManager;
            _commentManager = commentManager;
        }

        public IActionResult BoardList()
        {
            var boards = _boardManager.GetAll();
            var vm = new BoardListViewModel { Boards = boards };
            SetTitle("Táblák");
            return View(vm);
        }

        public IActionResult New()
        {
            return Board(0);
        }

        public IActionResult Board(int id)
        {
            return View(nameof(Board));
        }

        #region DELETE THIS
        // TODO VERY UGLY PLEASE CHANGE IT!!!!
        public IActionResult Test()
        {
            var rnd = new System.Random();
            var boardIds = new List<int>();

            for (int i = 0; i < 5; i++)
            {
                var cardList = new CardListDTO
                {
                    Name = "Test card list",
                    Cards = new List<CardDTO>()
                };

                var label = new LabelDTO
                {
                    Color = "#ff0000",
                    Name = "Red test label"
                };
                label.Id = _labelManager.Save(label);

                var comment = new CommentDTO
                {
                    Author = "Unknown",
                    Text = "This is a test comment, please ignore."
                };
                comment.Id = _commentManager.Save(comment);

                for (int k = 0; k < 3; k++)
                {
                    var card = new CardDTO
                    {
                        Description = $"Description of Test Card #{k}",
                        Name = $"Test card #{k}",
                        Comments = new List<CommentDTO>(),
                        Labels = new List<LabelDTO>()
                    };
                    card.Labels.Add(label);
                    card.Comments.Add(comment);
                    card.Id = _cardManager.Save(card);
                    cardList.Cards.Add(card);
                }
                cardList.Id = _cardListManager.Save(cardList);

                var board = new BoardDTO
                {
                    CardLists = new List<CardListDTO>(),
                    Type = (BoardType)rnd.Next(0, 2),
                    Name = $"Test Board #{i + 1}"
                };

                board.CardLists.Add(cardList);
                board.Id = _boardManager.Save(board);
                boardIds.Add(board.Id);
            }

            var b = _boardManager.Get(boardIds);
            return Json(b);
        }

        public IActionResult Test2(int id)
        {
            var board = _boardManager.Get(id);
            var comment = new CommentDTO
            {
                Author = "Sickae",
                Text = "This is a changed text"
            };

            board.CardLists[0].Cards[0].Comments[0] = comment;
            _boardManager.Save(board);

            var b = _boardManager.Get(id);
            return Json(b);
        }
        #endregion
    }
}