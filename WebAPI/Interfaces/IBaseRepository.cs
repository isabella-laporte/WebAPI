using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IQueryable<T>> Get(int page, int maxResults);
        Task<T?> GetByKey(int id);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<int> Delete(int id);
    }

    }
