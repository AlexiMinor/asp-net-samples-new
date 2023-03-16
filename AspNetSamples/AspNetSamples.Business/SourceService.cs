using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            return await _unitOfWork.Sources
                .GetAsQueryable()
                .Select(source => _mapper.Map<SourceDto>(source))
                .ToListAsync();
        }

        public async Task<SourceDto?> GetSourceIdsAsync(int id)
        {
            return _mapper.Map<SourceDto>(await _unitOfWork.Sources.GetByIdAsync(id));
        }

        public async Task<int> AddSourceAsync(SourceDto dto)
        {
            await _unitOfWork.Sources.AddAsync(_mapper.Map<Source>(dto));
            return await _unitOfWork.SaveChangesAsync();

        }

       
    }
}