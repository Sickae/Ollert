using System.Collections.Generic;

namespace Ollert.DataAccess.Entitites
{
    public class Category : Entity
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<Board> Boards { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}
