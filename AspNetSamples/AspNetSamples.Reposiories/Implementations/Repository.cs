using System.Linq.Expressions;
using AspNetSamples.Abstractions.Data;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Core;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseEntity
{
    protected readonly NewsAggregatorContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(NewsAggregatorContext newsAggregatorContext)
    {
        Db = newsAggregatorContext;
        DbSet = Db.Set<TEntity>();
    }


    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, 
        params Expression<Func<TEntity, object>>[] includes)
    {
        var result = DbSet.Where(predicate);
        if (includes.Any())
        {
            result = includes
                .Aggregate(result,
                    (current, include)
                        => current.Include(include));

            //foreach (var includeExpression in includes)
            //{
            //    result = result.Include(includeExpression);
            //}
        }
        return result;
    }

    public IQueryable<TEntity> GetAsQueryable()
    {
        return DbSet;
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task PatchAsync(int id, List<PatchDto> patchDtos)
    {
        var entity =
            await DbSet.FirstOrDefaultAsync(ent => ent.Id == id);

        var nameValuePairProperties = patchDtos.ToDictionary
        (k => k.PropertyName,
            v => v.PropertyValue);

        var dbEntityEntry = Db.Entry(entity);
        dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
        dbEntityEntry.State = EntityState.Modified;
    }

    public async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public async Task Remove(int id)
    {
        var entity =
            await DbSet.FirstOrDefaultAsync(ent => ent.Id==id);
        DbSet.Remove(entity);
    }

    public async Task RemoveRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }
}