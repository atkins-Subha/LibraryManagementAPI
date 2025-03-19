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
    public interface IFineService
    {

        Task<IEnumerable<Fine>> GetAllFinesService();
        Task<Fine> GetFineByIdService(int bookId);
        Task<Fine> CreateFineService(Fine book);
        Task UpdateFineService(Fine book);
        Task DeleteFineService(Fine book);

        Task<Response> CreateFineService(FineRequest request);
        Task<Response> DeleteFineService(int FineId);

    }
    public class FineService : IFineService
    {
        private readonly IFineRepository _FineRepository;
        private readonly IConfiguration _configuration;

        public FineService(IFineRepository FineRepository, IConfiguration configuration)
        {
            _FineRepository = FineRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Fine>> GetAllFinesService()
        {
            return await _FineRepository.GetAllFineRepo();
        }

        public async Task<Fine> GetFineByIdService(int FineId)
        {
            return await _FineRepository.GetFineByIdRepo(FineId);
        }


        public async Task<Fine> CreateFineService(Fine Fine)
        {
            return await _FineRepository.CreateFineRepo(Fine);
        }

        public async Task UpdateFineService(Fine Fine)
        {
            await _FineRepository.UpdateFineRepo(Fine);
        }
        public async Task DeleteFineService(Fine Fine)
        {
            await _FineRepository.DeleteFineRepo(Fine);
        }


        public async Task<Response> CreateFineService(FineRequest request)
        {
            Response response = new Response();
            try
            {
                var dbfine = _FineRepository.GetAllFineRepo().Result.FirstOrDefault(p => p.DaysRange.Trim().ToLower() == request.DaysRange.Trim().ToLower());

                if ((dbfine == null))
                {

                    var fine = new Fine
                    {
                        DaysRange = request.DaysRange,
                        FineAmount = request.FineAmount.Value
                    };
                    Fine newFine = await _FineRepository.CreateFineRepo(fine);
                    if (newFine != null)
                    {
                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.fineCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.fineNotCreatedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.fineAlreadyExistsMsg;
                return response;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
            }
            return response;

        }

        public async Task<Response> DeleteFineService(int FineId)
        {
            Response response = new Response();
            try
            {
                Fine Fine = await _FineRepository.GetFineByIdRepo(FineId);
                if (Fine != null)
                {
                    await _FineRepository.DeleteFineRepo(Fine);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.fineDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.fineNotDeletedMsg;
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
