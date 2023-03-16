using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class CommentGenericRepository : Repository<Comment>
{
    public CommentGenericRepository(NewsAggregatorContext newsAggregatorContext) 
        : base(newsAggregatorContext)
    {
    }
}