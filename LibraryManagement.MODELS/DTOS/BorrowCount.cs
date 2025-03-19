using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BorrowCount
    {
        public int UserId { get; set; }
        public int BooksAllowedCount { get; set; }
        public int TotalUserBorrowCount { get; set; }
    }
}
