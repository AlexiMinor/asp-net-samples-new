using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories
{
    public class CommentRepository : ICommentRepository 
    {
        private readonly NewsAggregatorContext _newsAggregatorContext;

        public CommentRepository(NewsAggregatorContext newsAggregatorContext)
        {
            _newsAggregatorContext = newsAggregatorContext;
        }

        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _newsAggregatorContext.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(comment => comment.Id == id);

            return 
                comment == null 
                    ? null 
                    : Convert(comment);
        }

        public async Task<List<CommentDto>> GetCommentsAsync()
        {
            var comments = await _newsAggregatorContext.Comments
                .AsNoTracking()
                .Select(comment => Convert(comment))
                .ToListAsync();

            return comments;
        }

        public async Task AddCommentAsync(CommentDto dto)
        {
            var entity = Convert(dto);
            await _newsAggregatorContext.Comments.AddAsync(entity);
            await _newsAggregatorContext.SaveChangesAsync();

        }

        public async Task AddCommentsAsync(IEnumerable<CommentDto> dtos)
        {
            var entities = dtos.Select(dto => Convert(dto)).ToList();
            await _newsAggregatorContext.Comments.AddRangeAsync(entities);
            await _newsAggregatorContext.SaveChangesAsync();
        }

        public async Task UpdateComment(CommentDto dto)
        {
            var ent = Convert(dto);

            var entForUpdate = await _newsAggregatorContext.Comments
                .FirstOrDefaultAsync(comment => comment.Id == ent.Id);

            if (entForUpdate!= null)
            {
                entForUpdate = ent;
            }

            await _newsAggregatorContext.SaveChangesAsync();

        }

        public async Task<int> CountAsync()
        {
            return await _newsAggregatorContext.Comments.CountAsync();
        }

        public async Task RemoveComment(CommentDto dto)
        {
            var ent = Convert(dto);
            _newsAggregatorContext.Comments.Remove(ent);
            await _newsAggregatorContext.SaveChangesAsync();
        }

        public async Task RemoveCommentsAsync(IEnumerable<CommentDto> dtos)
        {
            var ents = dtos.Select(dto => Convert(dto)).ToList();
            _newsAggregatorContext.Comments.RemoveRange(ents);
            await _newsAggregatorContext.SaveChangesAsync();
        }

        public async Task<List<CommentDto>> GetCommentsByArticleIdAsync(int articleId)
        {
            return await _newsAggregatorContext.Comments
                .Where(comment => comment.ArticleId.Equals(articleId))
                .Select(comment => Convert(comment))
                .ToListAsync();
        }

        public async Task<List<CommentDto>> GetCommentsForPageAsync(int page, int pageSize)
        {
            return await _newsAggregatorContext.Comments
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(comment => Convert(comment))
                .ToListAsync();
        }

        private static CommentDto Convert(Comment comment)
        {
            var dto = new CommentDto
            {
                Id = comment.Id,
                ArticleId = comment.ArticleId,
                CommentText = comment.CommentText,
            };

            return dto;
        }

        private static Comment Convert(CommentDto dto)
        {
            var comment = new Comment
            {
                Id = dto.Id,
                ArticleId = dto.ArticleId,
                CommentText = dto.CommentText,
            };

            return comment;
        }
    }
}