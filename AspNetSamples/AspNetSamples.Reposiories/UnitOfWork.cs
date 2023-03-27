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
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Source> _sourceRepository;
    private readonly IRepository<User> _userRepository;
    
    public UnitOfWork(NewsAggregatorContext dbContext, 
    IArticleRepository newArticleRepository, 
    IRepository<Comment> commentRepository, 
    IRepository<Source> sourceRepository, 
    IRepository<Role> roleRepository,
    IRepository<User> userRepository)
    {
        _dbContext = dbContext;
        _newArticleRepository = newArticleRepository;
        _commentRepository = commentRepository;
        _sourceRepository = sourceRepository;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public IArticleRepository Articles => _newArticleRepository;
    public IRepository<Comment> Comments => _commentRepository;
    public IRepository<Role> Roles => _roleRepository;
    public IRepository<Source> Sources => _sourceRepository;
    public IRepository<User> Users => _userRepository;

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