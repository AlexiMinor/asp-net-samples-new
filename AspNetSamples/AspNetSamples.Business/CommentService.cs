using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> GetCommentsByArticleIdAsync(int articleId)
    {
        return await _unitOfWork.Comments
            .FindBy(comment => comment.ArticleId == articleId)
            .Select(comment => _mapper.Map<CommentDto>(comment))
            .ToListAsync();
    }

    
}