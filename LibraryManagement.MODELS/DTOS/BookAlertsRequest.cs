using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BookAlertsRequest
    {
        public int? BookAlertsId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }

        public bool? IsBookBorrowedAlert { get; set; } = false;

        public bool? IsBookReturnedAlert { get; set; } = false;
    }
}
