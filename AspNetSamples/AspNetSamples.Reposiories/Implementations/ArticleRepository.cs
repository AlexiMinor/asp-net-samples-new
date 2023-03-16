using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(NewsAggregatorContext newsAggregatorContext) 
        : base(newsAggregatorContext)
    {
    }

    public async Task<List<Article>> GetArticlesByPageAsync(int page, int pageSize)
    {
        var articles = await DbSet
            .Include(article => article.Source)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return articles;
    }

    public override async Task AddRangeAsync(IEnumerable<Article> entities)
    {
        DbSet.AddRange(entities);
        //some other functionality here
    }

}