using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class SubscriptionDetailsRequest
    {
        public int? SubscriptionDetailsId { get; set; }
        public int? SubscriptionId { get; set; }
        public int? UserId { get; set; }
    }
}
