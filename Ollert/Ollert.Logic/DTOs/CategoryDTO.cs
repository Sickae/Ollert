using System.Collections.Generic;

namespace Ollert.Logic.DTOs
{
    public class CategoryDTO : DTOBase
    {
        public string Name { get; set; }

        public IList<BoardDTO> Boards { get; set; }
    }
}
