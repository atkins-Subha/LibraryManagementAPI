using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BookRequest
    {
        public int? BookId { get; set; }
        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? Genre { get; set; }

        public string? Publisher { get; set; }

        public string? ISBN { get; set; }

        public int? NoOfBooks { get; set; } = 0;

        public int? CategoryId { get; set; }
    }
}
