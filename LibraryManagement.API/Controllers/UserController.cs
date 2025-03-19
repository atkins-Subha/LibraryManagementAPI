using LibraryManagement.BUSINESS.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.MODELS.DTOS;
using Serilog;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.API.Extensions;
using System.Net;
using LibraryManagement.MODELS.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionDetailsService _subscriptionDetailsService;

        public UserController(IUserService userService, ISubscriptionDetailsService subscriptionDetailsService)
        {
            _userService = userService;
            _subscriptionDetailsService = subscriptionDetailsService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            Response response = new Response();
            try
            {
                var result = await _userService.GetAllUsersService();

                if (result != null)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = result;
                    return Ok(response);
                }
                response.statuscode = Constants.FailCode;
                response.data = result;
                response.message = Constants.NoDataFoundMsg;
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

        [HttpGet("GetAllUserswithSubscription")]
        public async Task<IActionResult> GetAllUserswithSubscription()
        {
            Response response = new Response();
            try
            {
                var result = (from users in _userService.GetAllUsersService().Result
                              join subscriptionDetails in _subscriptionDetailsService.GetAllSubscriptionDetailsService().Result
                              on users.UserId equals subscriptionDetails.UserId
                              select new
                              {
                                  user = users,
                                  role = users.Role,
                                  subscriptionDetail = subscriptionDetails
                              });

                if (result != null)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = result;
                    return Ok(response);
                }
                response.statuscode = Constants.FailCode;
                response.data = result;
                response.message = Constants.NoDataFoundMsg;
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

        [HttpGet("GetFilterUsersbyStatus")]
        public async Task<IActionResult> GetFilterUsersbyStatus([FromQuery] string status)
        {
            Response response = new Response();
            try
            {
              
                var lstAllUsers = _userService.GetAllUsersService().Result;

                if (!string.IsNullOrEmpty(status) && status == "All")
                {
                    lstAllUsers = lstAllUsers;
                }
                else if (!string.IsNullOrEmpty(status) && status == "Authenticated")
                {
                    lstAllUsers = lstAllUsers.Where(x => x.IsActive == true && x.IsApproved == true);
                }
                else if (!string.IsNullOrEmpty(status) && status == "UnApproved")
                {
                    lstAllUsers = lstAllUsers.Where(x => x.IsActive == true && x.IsApproved == false);
                }
                else if (!string.IsNullOrEmpty(status) && status == "Blocked")
                {
                    lstAllUsers = lstAllUsers.Where(x => x.IsActive == false && x.IsApproved == true);
                }

                var result = (from users in lstAllUsers
                                join subscriptionDetails in _subscriptionDetailsService.GetAllSubscriptionDetailsService().Result
                              on users.UserId equals subscriptionDetails.UserId
                              select new
                              {
                                  user = users,
                                  role = users.Role,
                                  subscriptionDetail = subscriptionDetails
                              }).ToList();


                if (result.Count > 0)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = result;
                    return Ok(response);
                }
                response.statuscode = Constants.FailCode;
                response.data = result;
                response.message = Constants.NoDataFoundMsg;
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

        [HttpGet("GetUnApprovedUsers")]
        public async Task<IActionResult> GetUnApprovedUsers()
        {
            Response response = new Response();
            try
            {

                var result = (from users in _userService.GetAllUsersService().Result.Where(x => x.IsApproved == false)
                              join subscriptionDetails in _subscriptionDetailsService.GetAllSubscriptionDetailsService().Result
                              on users.UserId equals subscriptionDetails.UserId
                              select new
                              {
                                  user = users,
                                  role = users.Role,
                                  subscriptionDetail = subscriptionDetails,
                                  selected = false
                              }).ToList();

                if (result.Count > 0)
                {
                    response.statuscode = Constants.SuccessCode;
                    response.data = result;
                    return Ok(response);
                }
                response.statuscode = Constants.FailCode;
                response.data = result;
                response.message = Constants.NoDataFoundMsg;
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

        [HttpGet("GetUnApprovedUsersCount")]
        public async Task<IActionResult> GetUnApprovedUsersCount()
        {
            Response response = new Response();
            try
            {
                var result = _userService.GetAllUsersService().Result.Where(x => x.IsApproved == false).Count();
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


        [HttpGet("GetUserbyId")]
        public async Task<IActionResult> GetUserbyId([FromQuery] int userId)
        {
            Response response = new Response();
            try
            {
                // Validate query parameter
                if (userId <= 0)
                {
                    return BadRequest("Invalid userId.");
                }

                var result = await _userService.GetUserByIdService(userId);

                if (result == null)
                {
                    return NotFound("User not found.");
                }
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

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(request.UserName))
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

            if (string.IsNullOrEmpty(request.UserEmail))
            {
                response.statuscode = Constants.FailCode;
                response.message = "UserEmail is Required!";
                return Ok(response);
            }

            if (request.RoleId.HasValue && request.RoleId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "RoleId is Required!";
                return Ok(response);
            }

            if (request.SubscriptionId.HasValue && request.SubscriptionId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "SubscriptionId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _userService.RegisterUser(request);
                //var result = "success";
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

        [HttpPut("UpdateUserUnApprovedStatus")]
        public async Task<IActionResult> UpdateUserUnApprovedStatus([FromBody] List<int> selectedUserIds)
        {
            Response response = new Response();

            try
            {

                foreach (var userId in selectedUserIds)
                {
                    await _userService.UpdateUserUnApprovedStatusService(userId);
                }

                response.statuscode = Constants.SuccessCode;
                response.message = "Approved Successfully";


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


        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int userId)
        {
            Response response = new Response();
            try
            {
                //var result = await _userService.DeleteUser(userId);
                var result = "success";
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
