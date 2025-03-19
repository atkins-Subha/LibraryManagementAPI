using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.MODELS.DTOS
{
    public class UpdateUserRequest
    {
        public int? UserId { get; set; }
        public int? PlanId { get; set; }

    }
}
