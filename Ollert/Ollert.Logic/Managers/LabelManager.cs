using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class LabelManager : ManagerBase<Label, LabelDTO>, ILabelManager
    {
        public LabelManager(ISession session) : base(session)
        { }
    }
}
