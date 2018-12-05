using System;

namespace Ollert.Logic.DTOs
{
    /// <summary>
    /// DTO a <see cref="Ollert.DataAccess.Entitites.Entity"/> entitáshoz
    /// </summary>
    public abstract class DTOBase
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        protected DTOBase()
        {
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }
    }
}
