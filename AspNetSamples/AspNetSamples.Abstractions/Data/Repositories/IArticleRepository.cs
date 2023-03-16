using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface IArticleRepository : IRepository<Article>
{
    public Task<List<Article>> GetArticlesByPageAsync(int page, int pageSize);
}