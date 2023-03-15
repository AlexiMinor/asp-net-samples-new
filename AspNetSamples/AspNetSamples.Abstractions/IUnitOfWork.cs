using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions;

public interface IUnitOfWork : IDisposable
{
    public IArticleRepository Articles { get; }
    public ICommentRepository Comments { get; }
    public ISourceRepository Sources { get; }
    public IRepository<Article> NewArticleRepository { get; }
    Task<int> SaveChangesAsync();
}