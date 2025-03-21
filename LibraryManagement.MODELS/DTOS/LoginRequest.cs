﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.DTOS
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Please enter the UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        public string Password { get; set; }
    }
}
