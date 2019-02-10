using Ollert.DataAccess.Attributes;
using System.Collections.Generic;

namespace Ollert.DataAccess.Entitites
{
    public class Card : Entity
    {
        public virtual string Name { get; set; }

        [MaxLength]
        public virtual string Description { get; set; }

        public virtual IEnumerable<Label> Labels { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
