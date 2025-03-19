using LibraryManagement.MODELS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class FineDetailsRequest
    {

        
        public int? FineDetailsId { get; set; }
        public int? FineId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string? Reason { get; set; }
        public DateTime? FineDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }



    }
}
