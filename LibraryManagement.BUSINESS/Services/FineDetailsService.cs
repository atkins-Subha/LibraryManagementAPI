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
    public interface IFineDetailsService
    {

        Task<IEnumerable<FineDetails>> GetAllFineDetailssService();
        Task<FineDetails> GetFineDetailsByIdService(int bookId);
        Task<FineDetails> CreateFineDetailsService(FineDetails book);
        Task UpdateFineDetailsService(FineDetails book);
        Task DeleteFineDetailsService(FineDetails book);

        Task<Response> CreateFineDetailsService(FineDetailsRequest request);
        Task<Response> DeleteFineDetailsService(int FineDetailsId);

    }
    public class FineDetailsService : IFineDetailsService
    {
        private readonly IFineDetailsRepository _FineDetailsRepository;
        private readonly IConfiguration _configuration;

        public FineDetailsService(IFineDetailsRepository FineDetailsRepository, IConfiguration configuration)
        {
            _FineDetailsRepository = FineDetailsRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<FineDetails>> GetAllFineDetailssService()
        {
            return await _FineDetailsRepository.GetAllFineDetailsRepo();
        }

        public async Task<FineDetails> GetFineDetailsByIdService(int FineDetailsId)
        {
            return await _FineDetailsRepository.GetFineDetailsByIdRepo(FineDetailsId);
        }


        public async Task<FineDetails> CreateFineDetailsService(FineDetails FineDetails)
        {
            return await _FineDetailsRepository.CreateFineDetailsRepo(FineDetails);
        }

        public async Task UpdateFineDetailsService(FineDetails FineDetails)
        {
            await _FineDetailsRepository.UpdateFineDetailsRepo(FineDetails);
        }
        public async Task DeleteFineDetailsService(FineDetails FineDetails)
        {
            await _FineDetailsRepository.DeleteFineDetailsRepo(FineDetails);
        }

        public async Task<Response> CreateFineDetailsService(FineDetailsRequest request)
        {
            Response response = new Response();
            try
            {
                var dbFineDetails = _FineDetailsRepository.GetAllFineDetailsRepo().Result.FirstOrDefault(p => p.UserId == request.UserId && p.BookId == request.BookId);

                if ((dbFineDetails == null))
                {

                    var fineDetails = new FineDetails
                    {

                        UserId = request.UserId.Value,
                        BookId = request.BookId.Value,
                        Reason = request.Reason,
                        FineDate = request.FineDate,
                        DueDate = request.DueDate,
                        PaidDate = request.PaidDate,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,

                    };
                    FineDetails newFineDetails = await _FineDetailsRepository.CreateFineDetailsRepo(fineDetails);
                    if (newFineDetails != null)
                    {
                        response.statuscode = Constants.SuccessCode;
                        response.data = newFineDetails;
                        response.message = Constants.fineDetailsCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.fineDetailsNotCreatedMsg;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.InnerException.Message;
                return response;
            }

            return response;

        }


        public async Task<Response> DeleteFineDetailsService(int FineDetailsId)
        {
            Response response = new Response();
            try
            {
                FineDetails FineDetails = await _FineDetailsRepository.GetFineDetailsByIdRepo(FineDetailsId);
                if (FineDetails != null)
                {
                    await _FineDetailsRepository.DeleteFineDetailsRepo(FineDetails);

                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.fineDetailsDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.fineDetailsNotDeletedMsg;
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
