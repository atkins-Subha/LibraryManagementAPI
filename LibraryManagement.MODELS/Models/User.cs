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
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter the UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter the Email Address")]
        public string UserEmail { get; set; }

        [ForeignKey("Role")]
        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public bool? IsApproved { get; set; } = false;
        public bool? IsActive { get; set; } = false;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
