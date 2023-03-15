using System.Linq.Expressions;
using AspNetSamples.Core;
using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface IRepository<T>: IDisposable
    where T : IBaseEntity
{
    //read 
    public Task<T?> GetByIdAsync(int id);
    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public IQueryable<T> GetAsQueryable();


    //Create
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    Task PatchAsync(int id, List<PatchDto> patchDtos);
    Task Update(T entity);

    Task Remove(int id);
    Task RemoveRange(IEnumerable<T> entities);
}