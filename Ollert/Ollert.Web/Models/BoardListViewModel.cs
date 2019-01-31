using Ollert.Logic.DTOs;
using System.Collections.Generic;

namespace Ollert.Web.Models
{
    public class BoardListViewModel
    {
        public IList<BoardDTO> Boards { get; set; }
    }
}
