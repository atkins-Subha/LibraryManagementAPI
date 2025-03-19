using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "Please enter the Category")]
        public string CategoryName { get; set; }
    }
}
