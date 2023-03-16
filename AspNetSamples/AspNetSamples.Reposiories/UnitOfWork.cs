using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NewsAggregatorContext _dbContext;
    private readonly IArticleRepository _newArticleRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Source> _sourceRepository;
    
    public UnitOfWork(NewsAggregatorContext dbContext, 
    IArticleRepository newArticleRepository, 
    IRepository<Comment> commentRepository, 
    IRepository<Source> sourceRepository)
    {
        _dbContext = dbContext;
        _newArticleRepository = newArticleRepository;
        _commentRepository = commentRepository;
        _sourceRepository = sourceRepository;
    }

    public IArticleRepository Articles => _newArticleRepository;
    public IRepository<Comment> Comments => _commentRepository;
    public IRepository<Source> Sources => _sourceRepository;

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _newArticleRepository?.Dispose();
        GC.SuppressFinalize(this);
    }
}