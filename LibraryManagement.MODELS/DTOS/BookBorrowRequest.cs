using LibraryManagement.MODELS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BookBorrowRequest
    {
        public int? BookBorrowId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }

        public int? StatusId { get; set; } = 0;
        public DateTime? BorrowedDate { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
