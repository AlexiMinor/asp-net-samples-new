using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions.Services
{
    public interface ISourceService
    {
        Task<List<Source>> GetSourcesAsync();
        Task<List<int>> GetSourceIdsAsync();
    }
}