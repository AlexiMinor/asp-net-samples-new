using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services
{
    public interface ISourceService
    {
        Task<List<SourceDto>> GetSourcesAsync();
        Task<SourceDto?> GetSourceIdsAsync(int id);
        Task<int> AddSourceAsync(SourceDto dto);
    }
}