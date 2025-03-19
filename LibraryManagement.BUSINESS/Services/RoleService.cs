using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;


namespace LibraryManagement.BUSINESS.Services
{
    public interface IRoleService
    {

        Task<IEnumerable<Role>> GetAllRolesService();
        Task<Role> GetRoleByIdService(int roleId);
        Task<Role> CreateRoleService(Role role);
        Task UpdateRoleService(Role role);
        Task DeleteRoleService(Role role);

        Task<Response> CreateRoleService(RoleRequest request);
        Task<Response> DeleteRoleService(int roleId);

    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public RoleService(IRoleRepository roleRepository, IConfiguration configuration)
        {
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Role>> GetAllRolesService()
        {
            return await _roleRepository.GetAllRolesRepo();
        }

        public async Task<Role> GetRoleByIdService(int roleId)
        {
            return await _roleRepository.GetRoleByIdRepo(roleId);
        }


        public async Task<Role> CreateRoleService(Role role)
        {
            return await _roleRepository.CreateRoleRepo(role);
        }

        public async Task UpdateRoleService(Role role)
        {
            await _roleRepository.UpdateRoleRepo(role);
        }
        public async Task DeleteRoleService(Role role)
        {
            await _roleRepository.DeleteRoleRepo(role);
        }

        public async Task<Response> CreateRoleService(RoleRequest request)
        {
            Response response = new Response();

            try
            {
                var dbRole = _roleRepository.GetAllRolesRepo().Result.FirstOrDefault(p => p.RoleName.Trim().ToLower() == request.RoleName.Trim().ToLower());

                if ((dbRole == null))
                {
                    List<string> lstRoles = new List<string>() { Constants.AdminRole, Constants.LibrarianRole, Constants.MemberRole };
                    var isCorrectRole = lstRoles.Any(x => x.Equals(request.RoleName.Trim()));

                    if (isCorrectRole)
                    {
                        var role = new Role
                        {
                            RoleName = request.RoleName,
                        };
                        Role newRole = await _roleRepository.CreateRoleRepo(role);
                        response.data = newRole;

                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.roleCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.roleNotCreatedMsg;
                    return response;

                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.roleAlreadyExistsMsg;
                
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
            }
            return response;

        }

        


        public async Task<Response> DeleteRoleService(int roleId)
        {
            Response response = new Response();
            try
            {
                Role role = await _roleRepository.GetRoleByIdRepo(roleId);
                if (role != null)
                {
                    await _roleRepository.DeleteRoleRepo(role);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.roleDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.roleNotDeletedMsg;
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
