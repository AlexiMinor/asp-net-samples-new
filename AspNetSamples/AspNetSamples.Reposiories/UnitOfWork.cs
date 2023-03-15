using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;

namespace AspNetSamples.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NewsAggregatorContext _dbContext;
    private readonly IArticleRepository _articleRepository;
    private readonly IRepository<Article> _newArticleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ISourceRepository _sourceRepository;
    
    public UnitOfWork(NewsAggregatorContext dbContext, 
        IArticleRepository articleRepository, 
        ICommentRepository commentRepository, 
        ISourceRepository sourceRepository, 
        IRepository<Article> newArticleRepository)
    {
        _dbContext = dbContext;
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _sourceRepository = sourceRepository;
        _newArticleRepository = newArticleRepository;
    }

    public IArticleRepository Articles => _articleRepository;
    public ICommentRepository Comments => _commentRepository;
    public ISourceRepository Sources => _sourceRepository;
    public IRepository<Article> NewArticleRepository => _newArticleRepository;

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