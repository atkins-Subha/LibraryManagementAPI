using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class GetBookAlertRequest
    {
        public bool? IsBookBorrowedAlert { get; set; } = false;

        public bool? IsBookReturnedAlert { get; set; } = false;

    }
}
