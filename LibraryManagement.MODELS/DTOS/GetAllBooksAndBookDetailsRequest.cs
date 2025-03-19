using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class GetAllBooksAndBookDetailsRequest
    {
        public int userId {  get; set; }

        public string? searchText { get; set; }
    }
}
