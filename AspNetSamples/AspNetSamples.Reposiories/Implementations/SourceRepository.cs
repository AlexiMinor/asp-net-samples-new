using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories
{
    public class SourceRepository : ISourceRepository 
    {
        private readonly NewsAggregatorContext _newsAggregatorContext;

        public SourceRepository(NewsAggregatorContext newsAggregatorContext)
        {
            _newsAggregatorContext = newsAggregatorContext;
        }

        public async Task<SourceDto?> GetSourceByIdAsync(int id)
        {
            var source = await _newsAggregatorContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(source => source.Id == id);

            return 
                source == null 
                    ? null 
                    : Convert(source);
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            var sources = await _newsAggregatorContext.Sources
                .AsNoTracking()
                .Select(source => Convert(source))
                .ToListAsync();

            return sources;
        }

        public async Task<int> AddSourceAsync(SourceDto dto)
        {
            var entity = Convert(dto);
            var entityEntry = await _newsAggregatorContext.Sources.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task AddSourcesAsync(IEnumerable<SourceDto> dtos)
        {
            var entities = dtos.Select(dto => Convert(dto)).ToList();
            await _newsAggregatorContext.Sources.AddRangeAsync(entities);
        }

        public async Task UpdateSource(SourceDto dto)
        {
            var ent = Convert(dto);

            var entForUpdate = await _newsAggregatorContext.Sources
                .FirstOrDefaultAsync(source => source.Id == ent.Id);

            if (entForUpdate!= null)
            {
                entForUpdate = ent;
            }


        }

        public async Task<int> CountAsync()
        {
            return await _newsAggregatorContext.Sources.CountAsync();
        }

        public async Task RemoveSource(SourceDto dto)
        {
            var ent = Convert(dto);
            _newsAggregatorContext.Sources.Remove(ent);
        }

        public async Task RemoveSourcesAsync(IEnumerable<SourceDto> dtos)
        {
            var ents = dtos.Select(dto => Convert(dto)).ToList();
            _newsAggregatorContext.Sources.RemoveRange(ents);
        }

        public async Task<List<SourceDto>> GetSourcesForPageAsync(int page, int pageSize)
        {
            return await _newsAggregatorContext.Sources
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(source => Convert(source))
                .ToListAsync();
        }


        private static SourceDto Convert(Source source)
        {
            var dto = new SourceDto
            {
                Id = source.Id,
                Name = source.Name
            };

            return dto;
        }

        private static Source Convert(SourceDto dto)
        {
            var source = new Source
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return source;
        }
    }
}