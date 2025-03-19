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
    
   public interface ISubscriptionService
    {

        Task<IEnumerable<Subscription>> GetAllSubscriptionService();
        Task<Subscription> GetSubscriptionByIdService(int subscriptionId);
        Task<Subscription> CreateSubscriptionService(Subscription subscription);
        Task UpdateSubscriptionService(Subscription subscription);
        Task DeleteSubscriptionService(Subscription subscription);

        Task<Response> CreateSubscriptionService(SubscriptionRequest request);
        Task<Response> DeleteSubscriptionService(int subscriptionId);

    }
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IConfiguration _configuration;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IConfiguration configuration)
        {
            _subscriptionRepository = subscriptionRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionService()
        {
            return await _subscriptionRepository.GetAllSubscriptionRepo();
        }

        public async Task<Subscription> GetSubscriptionByIdService(int subscriptionId)
        {
            return await _subscriptionRepository.GetSubscriptionByIdRepo(subscriptionId);
        }


        public async Task<Subscription> CreateSubscriptionService(Subscription subscription)
        {
            return await _subscriptionRepository.CreateSubscriptionRepo(subscription);
        }

        public async Task UpdateSubscriptionService(Subscription subscription)
        {
            await _subscriptionRepository.UpdateSubscriptionRepo(subscription);
        }
        public async Task DeleteSubscriptionService(Subscription subscription)
        {
            await _subscriptionRepository.DeleteSubscriptionRepo(subscription);
        }

        public async Task<Response> CreateSubscriptionService(SubscriptionRequest request)
        {
            Response response = new Response();
            try
            {
                var dbSubscription = _subscriptionRepository.GetAllSubscriptionRepo().Result.FirstOrDefault(p => p.SubscriptionName.Trim().ToLower() == request.SubscriptionName.Trim().ToLower());

                if ((dbSubscription == null))
                {
                    List<string> lstSubscription = new List<string>() { "Basic", "Premium" };
                    var isCorrectSubscription = lstSubscription.Any(x => x.Equals(request.SubscriptionName.Trim()));

                    if (isCorrectSubscription)
                    {
                        var subscription = new Subscription
                        {
                            SubscriptionName = request.SubscriptionName,
                            AnnualFee = request.AnnualFee,
                            NoOfBooksAllowed = request.NoOfBooksAllowed.Value,
                            NoOfDaysAllowed = request.NoOfDaysAllowed.Value,
                            ValidityDays = request.ValidityDays.Value,
                        };
                        Subscription newSubscription = await _subscriptionRepository.CreateSubscriptionRepo(subscription);
                        response.data = newSubscription;

                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.subscriptionCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.subscriptionNotCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.subscriptionAlreadyExistsMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
            }

            return response;

        }

        public async Task<Response> DeleteSubscriptionService(int subscriptionId)
        {
            Response response = new Response();
            try
            {
                Subscription subscription = await _subscriptionRepository.GetSubscriptionByIdRepo(subscriptionId);
                if (subscription != null)
                {
                    await _subscriptionRepository.DeleteSubscriptionRepo(subscription);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.subscriptionDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.subscriptionNotDeletedMsg;
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
