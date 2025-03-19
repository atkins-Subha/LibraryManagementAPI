using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Task<IEnumerable<Role>> GetAllRolesRepo();
        Task<Role> GetRoleByIdRepo(int roleId);
        Task<Role> CreateRoleRepo(Role role);
        Task UpdateRoleRepo(Role role);
        Task DeleteRoleRepo(Role role);
    }
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<Role>> GetAllRolesRepo()
        {
            return await FindAllBaseRepo()
            .OrderByDescending(e => e.RoleId)
            .ToListAsync();
        }

        public async Task<Role> GetRoleByIdRepo(int roleId)
        {
            return await FindByConditionBaseRepo(e => e.RoleId.Equals(roleId))
            .FirstOrDefaultAsync();
        }

        public async Task<Role> CreateRoleRepo(Role role)
        {
            return await CreateBaseRepo(role);
        }
        public async Task UpdateRoleRepo(Role role)
        {
            await UpdateBaseRepo(role);
        }

        public async Task DeleteRoleRepo(Role role)
        {
            await DeleteBaseRepo(role);
        }
    }
}
