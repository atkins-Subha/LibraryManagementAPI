using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class SubscriptionRequest
    {
        public int? SubscriptionId { get; set; }
        public string? SubscriptionName { get; set; }
        public Decimal? AnnualFee { get; set; }

        public int? NoOfBooksAllowed { get; set; }
        public int? NoOfDaysAllowed { get; set; }

        public int? ValidityDays { get; set; }
    }
}
