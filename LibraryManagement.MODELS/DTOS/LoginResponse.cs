using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public LoggedUserDTO LoggedUser { get; set; }

    }

    public class LoggedUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string  Role { get; set; }
    }
}
