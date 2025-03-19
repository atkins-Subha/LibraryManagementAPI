using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionId { get; set; }

        [Required]
        public string SubscriptionName { get; set; }

        [Required]
        public Decimal? AnnualFee { get; set; }

        [Required]
        public int NoOfBooksAllowed { get; set; }

        [Required]
        public int NoOfDaysAllowed { get; set; }

        [Required]
        public int ValidityDays { get; set; }
    }
}
