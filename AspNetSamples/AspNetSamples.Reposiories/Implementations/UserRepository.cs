using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(NewsAggregatorContext newsAggregatorContext) : base(newsAggregatorContext)
    {
    }
}