using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class SourceGenericRepository : Repository<Source>
{
    public SourceGenericRepository(NewsAggregatorContext newsAggregatorContext) 
        : base(newsAggregatorContext)
    {
    }
}