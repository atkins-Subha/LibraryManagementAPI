using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace LibraryManagement.MODELS.Models
{
    public class Fine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FineId { get; set; }
        public string DaysRange { get; set; }
        public int FineAmount { get; set; }

      


    }
}
