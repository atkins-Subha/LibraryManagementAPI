using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter the Title")]
        public string Title { get; set; }

        public string Author { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Please enter the ISBN")]
        public string ISBN { get; set; }
        public int NoOfBooks { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
