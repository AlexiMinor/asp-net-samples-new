using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;

namespace AspNetSamples.Mvc.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}