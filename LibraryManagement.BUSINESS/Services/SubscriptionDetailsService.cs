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
    public interface ISubscriptionDetailsService
    {

        Task<IEnumerable<SubscriptionDetails>> GetAllSubscriptionDetailsService();
        Task<SubscriptionDetails> GetSubscriptionDetailsByIdService(int bookId);
        Task<SubscriptionDetails> CreateSubscriptionDetailsService(SubscriptionDetails book);
        Task UpdateSubscriptionDetailsService(SubscriptionDetails book);
        Task DeleteSubscriptionDetailsService(SubscriptionDetails book);

        Task<Response> CreateSubscriptionDetailsService(SubscriptionDetailsRequest request);
        Task<Response> DeleteSubscriptionDetailsService(int SubscriptionDetailsId);

    }
    public class SubscriptionDetailsService : ISubscriptionDetailsService
    {
        private readonly ISubscriptionDetailsRepository _SubscriptionDetailsRepository;
        private readonly IConfiguration _configuration;

        public SubscriptionDetailsService(ISubscriptionDetailsRepository SubscriptionDetailsRepository, IConfiguration configuration)
        {
            _SubscriptionDetailsRepository = SubscriptionDetailsRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<SubscriptionDetails>> GetAllSubscriptionDetailsService()
        {
            return await _SubscriptionDetailsRepository.GetAllSubscriptionDetailsRepo();
        }

        public async Task<SubscriptionDetails> GetSubscriptionDetailsByIdService(int SubscriptionDetailsId)
        {
            return await _SubscriptionDetailsRepository.GetSubscriptionDetailsByIdRepo(SubscriptionDetailsId);
        }


        public async Task<SubscriptionDetails> CreateSubscriptionDetailsService(SubscriptionDetails SubscriptionDetails)
        {
            return await _SubscriptionDetailsRepository.CreateSubscriptionDetailsRepo(SubscriptionDetails);
        }

        public async Task UpdateSubscriptionDetailsService(SubscriptionDetails SubscriptionDetails)
        {
            await _SubscriptionDetailsRepository.UpdateSubscriptionDetailsRepo(SubscriptionDetails);
        }
        public async Task DeleteSubscriptionDetailsService(SubscriptionDetails SubscriptionDetails)
        {
            await _SubscriptionDetailsRepository.DeleteSubscriptionDetailsRepo(SubscriptionDetails);
        }

        public async Task<Response> CreateSubscriptionDetailsService(SubscriptionDetailsRequest request)
        {
            Response response = new Response();
            try
            {
                var dbSubscriptionDetails = _SubscriptionDetailsRepository.GetAllSubscriptionDetailsRepo().Result.FirstOrDefault(p => p.SubscriptionId == request.SubscriptionId && p.UserId == request.UserId);

                if ((dbSubscriptionDetails == null))
                {

                    var subscriptionDetails = new SubscriptionDetails
                    {

                        UserId = request.UserId.Value,
                        SubscriptionId = request.SubscriptionId.Value,

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,

                    };
                    SubscriptionDetails newSubscriptionDetails = await _SubscriptionDetailsRepository.CreateSubscriptionDetailsRepo(subscriptionDetails);
                    if (newSubscriptionDetails != null)
                    {
                        response.statuscode = Constants.SuccessCode;
                        response.data = newSubscriptionDetails;
                        response.message = Constants.subscriptionDetailsCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.subscriptionDetailsNotCreatedMsg;
                    return response;
                }

                response.statuscode = Constants.FailCode;
                response.message = Constants.subscriptionDetailsAlreadyExistsMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;

            }
            return response;

        }

        public async Task<Response> DeleteSubscriptionDetailsService(int SubscriptionDetailsId)
        {
            Response response = new Response();
            try
            {
                SubscriptionDetails SubscriptionDetails = await _SubscriptionDetailsRepository.GetSubscriptionDetailsByIdRepo(SubscriptionDetailsId);
                if (SubscriptionDetails != null)
                {
                    await _SubscriptionDetailsRepository.DeleteSubscriptionDetailsRepo(SubscriptionDetails);

                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.subscriptionDetailsDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.subscriptionDetailsNotDeletedMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = -1;
                response.message = ex.InnerException.Message;
            }

            return response;
        }
    }
}
