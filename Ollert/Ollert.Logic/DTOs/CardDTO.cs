using System.Collections.Generic;

namespace Ollert.Logic.DTOs
{
    public class CardDTO : DTOBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<LabelDTO> Labels { get; set; }

        public IList<CommentDTO> Comments { get; set; }
    }
}
