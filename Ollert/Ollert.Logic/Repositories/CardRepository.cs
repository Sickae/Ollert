using NHibernate;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Logic.Repositories.Interfaces;
using System;
using System.Linq;

namespace Ollert.Logic.Repositories
{
    public class CardRepository : CardManager, IUnitOfWorkRepository<CardDTO>
    {
        private readonly ICommentManager _commentManager;

        public CardRepository(ICommentManager commentManager, ISession session) : base(session)
        {
            _commentManager = commentManager;
        }

        public override int Save(CardDTO dto)
        {
            var unsavedComments = dto.Comments.Where(x => x.Id == 0).ToList();
            dto.Comments.ToList().RemoveAll(x => x.Id == 0);

            foreach (var comment in unsavedComments)
            {
                comment.Id = _commentManager.Save(comment);
            }

            dto.Comments.ToList().AddRange(unsavedComments);

            return base.Save(dto);
        }

        public int AddNewComment(int id, string author, string text)
        {
            if (string.IsNullOrWhiteSpace(author) || author.Length == 0)
            {
                author = "Anoním";
            }

            var card = Get(id);
            var comment = new CommentDTO
            {
                Author = author,
                Text = text
            };

            card.Comments.Add(comment);
            Save(card);

            return comment.Id;
        }
    }
}
