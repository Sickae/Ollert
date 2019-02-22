using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Ollert.Logic.Repositories
{
    public class CategoryRepository : CategoryManager, IUnitOfWorkRepository<CategoryDTO>
    {
        private readonly BoardRepository _boardRepository;

        public CategoryRepository(ISession session, BoardRepository boardRepository) : base(session)
        {
            _boardRepository = boardRepository;
        }

        public override int Save(CategoryDTO dto)
        {
            var unsavedBoards = dto.Boards.Where(x => x.Id == 0).ToList();
            dto.Boards.ToList().RemoveAll(x => x.Id == 0);

            foreach (var board in unsavedBoards)
            {
                board.Id = _boardRepository.Save(board);
            }

            dto.Boards.ToList().AddRange(unsavedBoards);

            return base.Save(dto);
        }

        public int AddNewBoard(int categoryId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name.Length > 0 && name.Length <= 255)
            {
                var category = Get(categoryId);

                if (category == null)
                {
                    return 0;
                }

                var board = new BoardDTO
                {
                    Name = name,
                    CardLists = new List<CardListDTO>()
                };

                category.Boards.Add(board);
                Save(category);

                return board.Id;
            }
            else
            {
                return 0;
            }
        }

        public bool RemoveBoard(int id)
        {
            var category = GetAll().FirstOrDefault(x => x.Boards.Any(b => b.Id == id));
            var toDelete = category?.Boards.FirstOrDefault(x => x.Id == id);

            if (category == null || toDelete == null)
            {
                return false;
            }

            category.Boards.Remove(toDelete);
            Save(category);

            return true;
        }
    }
}
