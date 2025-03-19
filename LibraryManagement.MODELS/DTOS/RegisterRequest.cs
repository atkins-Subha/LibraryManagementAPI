using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Please enter the UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter the Role Id")]
        public int? RoleId { get; set; }

        public int? SubscriptionId { get; set; }
        public bool? IsApproved { get; set; } = false;
        public bool? IsActive { get; set; } = true;


        [Required(ErrorMessage = "Please enter the Email")]
        public string UserEmail {  get; set; }
    }
}
