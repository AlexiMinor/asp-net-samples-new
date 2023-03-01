using AspNetSamples.Abstractions.Services;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business
{
    public class SourceService : ISourceService 
    {
        private readonly NewsAggregatorContext _dbContext;

        public SourceService(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Source>> GetSourcesAsync()
        {
            return await _dbContext.Sources.ToListAsync();
        }

        public async Task<List<int>> GetSourceIdsAsync()
        {
            return await _dbContext.Sources.AsNoTracking().Select(source => source.Id).ToListAsync();
        }
    }
}