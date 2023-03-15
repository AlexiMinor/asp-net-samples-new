using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface ISourceRepository
{
    public Task<SourceDto?> GetSourceByIdAsync(int id);

    public Task<List<SourceDto>> GetSourcesAsync();

    /// <summary>
    /// Insert Source to context with return id 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>id of inserted entity</returns>
    public Task<int> AddSourceAsync(SourceDto dto);
    public Task AddSourcesAsync(IEnumerable<SourceDto> dtos);

    public Task UpdateSource(SourceDto dto);
    public Task<int> CountAsync();

    public Task RemoveSource(SourceDto dto);
    public Task RemoveSourcesAsync(IEnumerable<SourceDto> dtos);

}