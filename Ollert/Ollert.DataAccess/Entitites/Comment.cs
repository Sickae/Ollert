namespace Ollert.DataAccess.Entitites
{
    public class Comment : Entity
    {
        public virtual string Author { get; set; }

        public virtual string Text { get; set; }
    }
}
