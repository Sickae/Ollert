using Ollert.Logic.DTOs;
using System.Collections.Generic;

namespace Ollert.Web.Models
{
    public class CategoryListViewModel
    {
        public IList<CategoryDTO> Categories { get; set; }
    }
}
