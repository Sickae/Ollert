using Ollert.DataAccess.Enums;
using System.Collections.Generic;

namespace Ollert.DataAccess.Entitites
{
    public class Board : Entity
    {
        public virtual string Name { get; set; }

        public virtual BoardType Type { get; set; }

        public virtual IEnumerable<CardList> CardLists { get; set; }
    }
}
