using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.CQS.Queries;
using AspNetSamples.Data.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetSamples.Business
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {

            var sources = await _mediator.Send(new GetAllSourcesQuery());

            return sources;
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