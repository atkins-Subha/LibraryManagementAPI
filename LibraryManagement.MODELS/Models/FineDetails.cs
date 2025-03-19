using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace LibraryManagement.MODELS.Models
{
    public class FineDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FineDetailsId { get; set; }

        [ForeignKey("Fine")]
        [Required]
        public int FineId { get; set; }
        public Fine Fine { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Book")]
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string Reason { get; set; }

        public DateTime? FineDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
       
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
