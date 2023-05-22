using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;

namespace AspNetSamples.WebAPI.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}