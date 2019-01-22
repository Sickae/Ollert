using System.Collections.Generic;

namespace Ollert.Logic.DTOs
{
    public class CardListDTO : DTOBase
    {
        public string Name { get; set; }

        public IList<CardDTO> Cards { get; set; }
    }
}
