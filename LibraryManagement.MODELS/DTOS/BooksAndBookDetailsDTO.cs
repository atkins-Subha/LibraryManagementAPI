using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BooksAndBookDetailsDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string CategoryName { get; set; }

        public int TotalBooksCount { get; set; }

        public int UserBorrowedCount { get; set; }

        public int AvailableBooksCount { get; set; }

        public int ConfirmedUserBorrowedCount { get; set; }

        

    }
}
