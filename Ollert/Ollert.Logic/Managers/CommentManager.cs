using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class CommentManager : ManagerBase<Comment, CommentDTO>, ICommentManager
    {
        public CommentManager(ISession session) : base(session)
        { }
    }
}
