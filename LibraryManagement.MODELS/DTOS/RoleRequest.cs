using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class RoleRequest
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
