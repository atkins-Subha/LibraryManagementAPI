
using System.Linq.Expressions;

namespace LibraryManagement.DATAACCESS.Interfaces
{

    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAllBaseRepo();
        IQueryable<T> FindByConditionBaseRepo(Expression<Func<T, bool>> expression);
        Task<T> CreateBaseRepo(T entity);
        Task UpdateBaseRepo(T entity);
        Task DeleteBaseRepo(T entity);
    }
}
