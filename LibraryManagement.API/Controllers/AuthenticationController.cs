using LibraryManagement.BUSINESS.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.MODELS.DTOS;
using Serilog;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.API.Extensions;
using System.Net;
using LibraryManagement.MODELS.Models;
using System.Net.Mail;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(request.Username))
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserName is Required!";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                response.statuscode = Constants.FailCode;
                response.message = "Password is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _userService.AuthenticateUser(request);

                //var adminEmail = "subha.subramanian@atkinsrealis.com";
                //var mailMessage = new MailMessage("no-reply@example.com", adminEmail)
                //{
                //    Subject = "New User Registration Approval",
                //    //Body = $"A new user has registered. Please review and approve the registration.\n\nUsername: {user.Username}\nEmail: {user.Email}"
                //    Body = "Welcome"
                //};

                //using (var smtpClient = new SmtpClient("smtp.example.com"))
                //{
                //    smtpClient.Send(mailMessage);
                //}

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
           
        }
    }
}
