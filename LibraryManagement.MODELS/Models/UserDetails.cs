using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibraryManagement.MODELS.Models
{
    public class UserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserDetailsId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }


        [Required(ErrorMessage = "Please enter the First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Last Name")]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
