using System.Collections.Generic;

namespace Ollert.DataAccess.Entitites
{
    public class Card : Entity
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual IEnumerable<Label> Labels { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
