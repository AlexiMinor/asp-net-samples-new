using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;

namespace AspNetSamples.Mvc.MappingProfiles;

public class SourceProfile : Profile
{
    public SourceProfile()
    {
        CreateMap<Source, SourceDto>();
        CreateMap<SourceDto, Source>();
    }
}