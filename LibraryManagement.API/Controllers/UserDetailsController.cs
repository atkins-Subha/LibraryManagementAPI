using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/userdetails")]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsService _UserDetailsService;

        public UserDetailsController(IUserDetailsService UserDetailsService)
        {
            _UserDetailsService = UserDetailsService;
        }

        [HttpGet("GetAllUserkDetails")]
        public async Task<IActionResult> GetAllUserkDetails()
        {
            Response response = new Response();
            try
            {
                var result = await _UserDetailsService.GetAllUserDetailsService();
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

        [HttpGet("GetUserkDetailsbyUserId")]
        public async Task<IActionResult> GetUserkDetailsbyUserId([FromQuery] int userId)
        {
            Response response = new Response();
            try
            {
                var result = _UserDetailsService.GetAllUserDetailsService().Result.Where(x => x.UserId == userId).ToList();
                if (result.Count == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.NoDataFoundMsg;
                    return Ok(response);
                }

                response.statuscode = Constants.SuccessCode;
                response.data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }

        [HttpGet("GetUserDetailsbyId")]
        public async Task<IActionResult> GetUserDetailsbyId([FromQuery] int userDetailsId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (userDetailsId == 0)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = "User Detail Id is Required!";
                    return Ok(response);
                }

                var result = await _UserDetailsService.GetUserDetailsByIdService(userDetailsId);

                if (result == null)
                {
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.NoDataFoundMsg;
                    return Ok(response);
                }

                response.statuscode = Constants.SuccessCode;
                response.data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }

        //[Authorize(Roles = "Librarian")]
        [HttpPost("CreateUserDetails")]
        public async Task<IActionResult> CreateUserDetails([FromBody] UserDetailsRequest request)
        {
            Response response = new Response();

            if (request.UserId.HasValue && request.UserId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserId is Required!";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.FirstName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "FirstName is Required!";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "LastName is Required!";
                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                response.statuscode = Constants.FailCode;
                response.message = "PhoneNumber is Required!";
                return Ok(response);
            }
            if (string.IsNullOrEmpty(request.Address))
            {
                response.statuscode = Constants.FailCode;
                response.message = "Address is Required!";
                return Ok(response);
            }

            try
            {
                var result = await _UserDetailsService.CreateUserDetailsService(request);
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
        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserDetailsRequest request)
        {
            Response response = new Response();
            if (request.UserDetailsId.HasValue && request.UserDetailsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserDetailsId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _UserDetailsService.UpdateUserDetailsService(request);
                //var result = "";
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

        //[Authorize(Roles = "Librarian")]
        [HttpDelete("DeleteUserDetails")]
        public async Task<IActionResult> DeleteUserDetails(int UserDetailsId)
        {
            Response response = new Response();
            if (UserDetailsId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserDetailsId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _UserDetailsService.DeleteUserDetailsService(UserDetailsId);
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
