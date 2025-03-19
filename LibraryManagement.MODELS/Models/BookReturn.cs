using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.Models
{
    public class BookReturn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookReturnId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Book")]
        [Required]
        public int? BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("Status")]
        public int? StatusId { get; set; }
        public Status Status { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
