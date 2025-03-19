using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class StatusRequest
    {
        [Required(ErrorMessage = "Please enter the Status")]
        public string? StatusName { get; set; }
    }
}
