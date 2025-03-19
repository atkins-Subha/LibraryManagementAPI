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
    
    public interface IStatusService
    {

        Task<IEnumerable<Status>> GetAllStatusService();
        Task<Status> GetStatusByIdService(int StatusId);
        Task<Status> CreateStatusService(Status Status);
        Task UpdateStatusService(Status Status);
        Task DeleteStatusService(Status Status);

        Task<Response> CreateStatusService(StatusRequest request);
        Task<Response> DeleteStatusService(int StatusId);

    }
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _StatusRepository;
        private readonly IConfiguration _configuration;

        public StatusService(IStatusRepository StatusRepository, IConfiguration configuration)
        {
            _StatusRepository = StatusRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Status>> GetAllStatusService()
        {
            return await _StatusRepository.GetAllStatusRepo();
        }

        public async Task<Status> GetStatusByIdService(int StatusId)
        {
            return await _StatusRepository.GetStatusByIdRepo(StatusId);
        }


        public async Task<Status> CreateStatusService(Status Status)
        {
            return await _StatusRepository.CreateStatusRepo(Status);
        }

        public async Task UpdateStatusService(Status Status)
        {
            await _StatusRepository.UpdateStatusRepo(Status);
        }
        public async Task DeleteStatusService(Status Status)
        {
            await _StatusRepository.DeleteStatusRepo(Status);
        }

        public async Task<Response> CreateStatusService(StatusRequest request)
        {
            Response response = new Response();
            try
            {
                var dbStatus = _StatusRepository.GetAllStatusRepo().Result.FirstOrDefault(p => p.StatusName.Trim().ToLower() == request.StatusName.Trim().ToLower());

                if ((dbStatus == null))
                {

                    List<string> lstStatus = new List<string>() { "Borrowed", "Returned", "Overdue", "Damaged", "Lost", "Reserved" };
                    var isCorrectStatus = lstStatus.Any(x => x.Equals(request.StatusName.Trim()));

                    if (isCorrectStatus)
                    {
                        var  status= new Status
                        {
                            StatusName = request.StatusName,
                        };

                        Status newStatus = await _StatusRepository.CreateStatusRepo(status);
                        response.data = newStatus;

                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.statusCreatedMsg;
                        return response;

                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.statusNotCreatedMsg;
                    return response;

                }

                response.statuscode = Constants.FailCode;
                response.message = Constants.statusAlreadyExistsMsg;
            } 
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response> DeleteStatusService(int StatusId)
        {
            Response response = new Response();
            try
            {
                Status Status = await _StatusRepository.GetStatusByIdRepo(StatusId);
                if (Status != null)
                {
                    await _StatusRepository.DeleteStatusRepo(Status);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.statusDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.statusNotDeletedMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = -1;
                response.message = ex.InnerException.Message;
                return response;
            }

            return response;
        }
    }
}
