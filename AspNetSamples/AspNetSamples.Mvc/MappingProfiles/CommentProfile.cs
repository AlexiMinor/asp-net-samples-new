using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;

namespace AspNetSamples.Mvc.MappingProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();
    }
}