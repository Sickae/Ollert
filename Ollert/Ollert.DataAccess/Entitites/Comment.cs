using Ollert.DataAccess.Attributes;

namespace Ollert.DataAccess.Entitites
{
    public class Comment : Entity
    {
        public virtual string Author { get; set; }

        [MaxLength]
        public virtual string Text { get; set; }
    }
}
