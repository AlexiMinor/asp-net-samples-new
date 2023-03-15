using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class ArticleGenericRepository : Repository<Article>
{
    public ArticleGenericRepository(NewsAggregatorContext newsAggregatorContext) 
        : base(newsAggregatorContext)
    {
    }
}