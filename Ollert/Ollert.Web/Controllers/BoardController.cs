using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.DTOs;
using Ollert.Logic.Repositories;
using Ollert.Web.Models;
using System.Collections.Generic;

namespace Ollert.Web.Controllers
{
    public class BoardController : ControllerBase
    {
        private readonly BoardRepository _boardRepository;

        public BoardController(BoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public IActionResult New()
        {
            return Board(0);
        }

        public IActionResult Board(int id)
        {
            var board = _boardRepository.Get(id);
            var vm = Mapper.Map<BoardViewModel>(board);
            SetTitle(board.Name);
            return View(nameof(Board), vm);
        }

        public IActionResult AddNewCardList(int boardId, string cardListName)
        {
            if (!string.IsNullOrWhiteSpace(cardListName) && cardListName.Length > 0 && cardListName.Length <= 255)
            {
                var cardList = new CardListDTO
                {
                    Name = cardListName,
                    Cards = new List<CardDTO>()
                };

                var board = _boardRepository.Get(boardId);
                board.CardLists.Add(cardList);
                _boardRepository.Save(board);

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

        public IActionResult RemoveCardList(int boardId, int cardListId)
        {
            if (_boardRepository.RemoveCardList(boardId, cardListId))
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