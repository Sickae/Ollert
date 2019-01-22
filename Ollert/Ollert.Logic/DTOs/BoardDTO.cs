using Ollert.DataAccess.Enums;
using System.Collections.Generic;

namespace Ollert.Logic.DTOs
{
    public class BoardDTO : DTOBase
    {
        public string Name { get; set; }

        public BoardType Type { get; set; }

        public IList<CardListDTO> CardLists { get; set; }
    }
}
