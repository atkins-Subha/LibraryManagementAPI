using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LibraryManagement.MODELS.Constants;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.DATAACCESS.Repository;


namespace LibraryManagement.BUSINESS.Services
{
    public interface IUserService
    {

        Task<IEnumerable<User>> GetAllUsersService();
        Task<User> GetUserByIdService(int userID);
        Task<User> CreateUserService(User user);
        Task UpdateUserService(User user);
        Task DeleteUserService(User user);


        Task<Response> RegisterUser(RegisterRequest request);

        Task<string> UpdateUserUnApprovedStatusService(int userId);
        Task<string> DeleteUser(int userId);

        Task<LoginResponse> AuthenticateUser(LoginRequest request);
       
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionDetailsRepository _subscriptionDetailsRepository;

        public UserService(IUserRepository userRepository, IConfiguration configuration, ISubscriptionDetailsRepository subscriptionDetailsRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _subscriptionDetailsRepository = subscriptionDetailsRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersService()
        {
            return await _userRepository.GetAllUsersRepo();
        }

        public async Task<User> GetUserByIdService(int userId)
        {
            return await _userRepository.GetUserByIdRepo(userId);
        }


        public async Task<User> CreateUserService(User user)
        {
            return await _userRepository.CreateUserRepo(user);
        }

        public async Task UpdateUserService(User user)
        {
            await _userRepository.UpdateUserRepo(user);
        }
        public async Task DeleteUserService(User user)
        {
            await _userRepository.DeleteUserRepo(user);
        }

        public async Task<Response> RegisterUser(RegisterRequest request)
        {
            Response response = new Response();
            try
            {
                var dbUser = _userRepository.GetAllUsersRepo().Result.FirstOrDefault(p => p.UserName.Trim() == request.UserName.Trim() && p.UserEmail.Trim() == request.UserEmail.Trim());

                // Hash password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                if ((dbUser == null))
                {
                    try
                    {
                        var user = new User
                        {
                            UserName = request.UserName,
                            Password = hashedPassword,
                            RoleId = request.RoleId.Value,
                            UserEmail = request.UserEmail,

                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        };
                        User newUser = await _userRepository.CreateUserRepo(user);

                        if (newUser != null)
                        {
                            var subscriptionDetails = new SubscriptionDetails
                            {
                                SubscriptionId = request.SubscriptionId.Value,
                                UserId = newUser.UserId,

                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                            };
                            SubscriptionDetails newSubscriptionDetails = await _subscriptionDetailsRepository.CreateSubscriptionDetailsRepo(subscriptionDetails);
                            if (newSubscriptionDetails != null)
                            {
                                response.statuscode = Constants.SuccessCode;
                                response.data = newUser;
                                response.message = Constants.userCreatedMsg;
                                return response;
                            }

                            response.statuscode = Constants.FailCode;
                            response.message = Constants.userNotCreatedMsg;

                        }
                        response.statuscode = Constants.FailCode;
                        response.message = Constants.subscriptionDetailsNotCreatedMsg;

                    }
                    catch (Exception ex)
                    {
                        response.statuscode = Constants.ErrorCode;
                        response.message = ex.Message;
                    }
                    
                }
                else
                {
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.userAlreadyExistsMsg;
                }                

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }

            return response;

        }

        public async Task<string> UpdateUserUnApprovedStatusService(int userId)
        {

            var dbUser = _userRepository.GetUserByIdRepo(userId).Result;


            if ((dbUser != null))
            {
                try
                {
                    dbUser.IsApproved = true;
                    dbUser.Role = null;

                    dbUser.UpdatedAt = DateTime.UtcNow;

                    await _userRepository.UpdateUserRepo(dbUser);
                    return Constants.userUpdatedMsg;

                }
                catch (Exception ex)
                {
                    return ex.InnerException.Message;
                }
               
            }
            return Constants.userNotUpdatedMsg;

        }
        public async Task<string> DeleteUser(int userId)
        {
            User user = await _userRepository.GetUserByIdRepo(userId);
            await _userRepository.DeleteUserRepo(user);
            return Constants.userDeletedMsg;

        }

        public async Task<LoginResponse> AuthenticateUser(LoginRequest request)
        {

            var dbUser = _userRepository.GetAllUsersRepo().Result.FirstOrDefault(p => p.UserName.Trim() == request.Username.Trim() && p.IsApproved == true && p.IsActive == true);

            if (dbUser != null)
            {

                if (BCrypt.Net.BCrypt.Verify(request.Password, dbUser.Password))
                {
                    // Generate JWT
                    var token = GenerateJwt(dbUser);

                    LoggedUserDTO loggedUserDTO = new LoggedUserDTO()
                    {
                        UserId = dbUser.UserId,
                        UserName = dbUser.UserName,
                        UserEmail = dbUser.UserEmail,
                        Role = dbUser.Role.RoleName
                    };

                    return new LoginResponse { Token = token, Message = Constants.AuthenticateUserMsg, StatusCode = Constants.SuccessCode, LoggedUser = loggedUserDTO };
                }
            }

            LoggedUserDTO user = new LoggedUserDTO();
            return new LoginResponse { Token = "", Message = Constants.UnauthorizedUserMsg, StatusCode = Constants.FailCode, LoggedUser = user };


        }

        private string GenerateJwt(User user)
        {

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
