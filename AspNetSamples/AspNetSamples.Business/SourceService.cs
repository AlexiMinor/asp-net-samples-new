using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Repositories;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Business
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            return await _unitOfWork.Sources.GetSourcesAsync();
        }

        public async Task<SourceDto?> GetSourceIdsAsync(int id)
        {
            return await _unitOfWork.Sources.GetSourceByIdAsync(id);
        }

        public async Task<int> AddSourceAsync(SourceDto dto)
        {
            await _unitOfWork.Sources.AddSourceAsync(dto);
             return await _unitOfWork.SaveChangesAsync();

        }
    }
}