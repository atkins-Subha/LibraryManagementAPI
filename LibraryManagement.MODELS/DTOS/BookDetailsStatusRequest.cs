using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class BookDetailsStatusRequest
    {
        public int BookDetailsId { get; set; }
        public DateTime ReturnedDate { get; set; }
        public int StatusId { get; set; }
    }
}
