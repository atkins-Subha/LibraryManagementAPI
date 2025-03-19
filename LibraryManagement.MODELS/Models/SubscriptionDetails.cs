using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.Models
{
    public class SubscriptionDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionDetailsId { get; set; }

        [ForeignKey("Subscription")]
        [Required]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }


        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
