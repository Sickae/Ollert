using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Logic.Repositories;
using Ollert.Web.Models;

namespace Ollert.Web.Controllers
{
    public class BoardController : ControllerBase
    {
        private readonly BoardRepository _boardRepository;

        public BoardController(BoardRepository boardRepository, ICategoryManager categoryManager) : base(categoryManager)
        {
            _boardRepository = boardRepository;
        }

        public IActionResult New(int categoryId)
        {
            var board = new BoardDTO
            {
                Category = _categoryManager.Get(categoryId)
            };
            var vm = Mapper.Map<BoardViewModel>(board);
            return Board(vm);
        }

        public IActionResult Board(int id)
        {
            var board = _boardRepository.Get(id) ?? new BoardDTO();
            var vm = Mapper.Map<BoardViewModel>(board);
            return Board(vm);
        }

        private IActionResult Board(BoardViewModel model)
        {
            SetTitle(model.Name);
            FillViewBags();
            return View(nameof(Board), model);
        }

        public IActionResult AddNewCardList(int boardId, string cardListName)
        {
            var id = _boardRepository.AddNewCardList(boardId, cardListName);

            if (id > 0)
            {
                return Json(new
                {
                    success = true,
                    id,
                    name = cardListName
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

        public IActionResult Rename(int id, string name)
        {
            return Json(new
            {
                success = _boardRepository.Rename(id, name)
            });
        }
    }
}