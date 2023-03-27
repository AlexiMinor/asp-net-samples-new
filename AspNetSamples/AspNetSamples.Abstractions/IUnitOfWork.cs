using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions;

public interface IUnitOfWork : IDisposable
{
    public IArticleRepository Articles { get; }
    public IRepository<Comment> Comments { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Source> Sources { get; }
    public IRepository<User> Users { get; }
    public Task<int> SaveChangesAsync();

}