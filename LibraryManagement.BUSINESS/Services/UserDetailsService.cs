using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUSINESS.Services
{
    public interface IUserDetailsService
    {

        Task<IEnumerable<UserDetails>> GetAllUserDetailsService();
        Task<UserDetails> GetUserDetailsByIdService(int bookId);
        Task<UserDetails> CreateUserDetailsService(UserDetails book);
        Task UpdateUserDetailsService(UserDetails book);
        Task DeleteUserDetailsService(UserDetails book);

        Task<Response> CreateUserDetailsService(UserDetailsRequest request);

        Task<Response> UpdateUserDetailsService(UserDetailsRequest request);
        Task<Response> DeleteUserDetailsService(int UserDetailsId);

    }
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailsRepository _UserDetailsRepository;
        private readonly IConfiguration _configuration;

        public UserDetailsService(IUserDetailsRepository UserDetailsRepository, IConfiguration configuration)
        {
            _UserDetailsRepository = UserDetailsRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserDetails>> GetAllUserDetailsService()
        {
            return await _UserDetailsRepository.GetAllUserDetailsRepo();
        }

        public async Task<UserDetails> GetUserDetailsByIdService(int UserDetailsId)
        {
            return await _UserDetailsRepository.GetUserDetailsByIdRepo(UserDetailsId);
        }


        public async Task<UserDetails> CreateUserDetailsService(UserDetails UserDetails)
        {
            return await _UserDetailsRepository.CreateUserDetailsRepo(UserDetails);
        }

        public async Task UpdateUserDetailsService(UserDetails UserDetails)
        {
            await _UserDetailsRepository.UpdateUserDetailsRepo(UserDetails);
        }
        public async Task DeleteUserDetailsService(UserDetails UserDetails)
        {
            await _UserDetailsRepository.DeleteUserDetailsRepo(UserDetails);
        }

        public async Task<Response> CreateUserDetailsService(UserDetailsRequest request)
        {
            Response response = new Response();
            try
            {
                var dbUserDetails = _UserDetailsRepository.GetAllUserDetailsRepo().Result.FirstOrDefault(p => p.UserId == request.UserId);


                if ((dbUserDetails == null))
                {
                    try
                    {

                        var userDetails = new UserDetails
                        {

                            UserId = request.UserId.Value,
                            FirstName = request.FirstName,
                            LastName = request.LastName,
                            PhoneNumber = request.PhoneNumber,
                            Address = request.Address,

                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,

                        };
                        UserDetails newUserDetails = await _UserDetailsRepository.CreateUserDetailsRepo(userDetails);
                        if (newUserDetails != null)
                        {
                            response.statuscode = Constants.SuccessCode;
                            response.data = newUserDetails;
                            response.message = Constants.userDetailsCreatedMsg;
                            return response;
                        }
                        response.statuscode = Constants.FailCode;
                        response.message = Constants.userDetailsNotCreatedMsg;

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
                    response.message = Constants.userDetailsAlreadyExistsMsg;
                }

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }

            return response;

        }


        public async Task<Response> UpdateUserDetailsService(UserDetailsRequest request)
        {

            Response response = new Response();
            try
            {
                var dbUserDetails = _UserDetailsRepository.GetUserDetailsByIdRepo(request.UserDetailsId.Value).Result;


                if ((dbUserDetails != null))
                {
                    try
                    {
                        if (request.UserDetailsId.HasValue && request.UserDetailsId > 0)
                        {
                            dbUserDetails.UserDetailsId = request.UserDetailsId.Value;
                        }
                        if (request.UserId.HasValue && request.UserId > 0)
                        {
                            dbUserDetails.UserId = request.UserId.Value;
                        }
                        if (string.IsNullOrEmpty(request.FirstName))
                        {
                            dbUserDetails.FirstName = request.FirstName;
                        }
                        if (string.IsNullOrEmpty(request.LastName))
                        {
                            dbUserDetails.LastName = request.LastName;
                        }
                        if (string.IsNullOrEmpty(request.PhoneNumber))
                        {
                            dbUserDetails.PhoneNumber = request.PhoneNumber;
                        }
                        if (string.IsNullOrEmpty(request.Address))
                        {
                            dbUserDetails.Address = request.Address;
                        }


                        dbUserDetails.CreatedAt = DateTime.UtcNow;
                        dbUserDetails.UpdatedAt = DateTime.UtcNow;

                        await _UserDetailsRepository.UpdateUserDetailsRepo(dbUserDetails);

                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.userDetailsUpdatedMsg;
                        return response;

                    }
                    catch (Exception ex)
                    {
                        response.statuscode = Constants.ErrorCode;
                        response.message = ex.InnerException.Message;
                       
                    }

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.userDetailsNotExitsMsg;

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }

            return response;
        }

        public async Task<Response> DeleteUserDetailsService(int UserDetailsId)
        {
            Response response = new Response();
            try
            {
                UserDetails UserDetails = await _UserDetailsRepository.GetUserDetailsByIdRepo(UserDetailsId);
                if (UserDetails != null)
                {
                    await _UserDetailsRepository.DeleteUserDetailsRepo(UserDetails);

                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.userDetailsDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.userDetailsNotDeletedMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.InnerException.Message;
                return response;
            }

            return response;
        }
    }
}
