using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class RoleRepository : Repository<Role>
{
    public RoleRepository(NewsAggregatorContext newsAggregatorContext) : base(newsAggregatorContext)
    {
    }
}