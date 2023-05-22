using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AspNetSamples.WebAPI.Requests;
using AspNetSamples.WebAPI.Responses;
using AutoMapper;

namespace AspNetSamples.Mvc.MappingProfiles;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dto => dto.ShortDescription,
                opt
                    => opt.MapFrom(
                        article
                            => article.ShortDescription)); 
        //source -> destination
        CreateMap<Article, AutoCompleteDataDto>()
            .ForMember(dto => dto.Label,
                opt
                    => opt.MapFrom(
                        article
                            => article.Title))
            .ForMember(dto=>dto.Value, opt=>opt.MapFrom(article => article.Id)); //source -> destination

        CreateMap<ArticleDto, Article>()
            .ForMember(dto => dto.ShortDescription,
                opt
                    => opt.MapFrom(
                        dto
                            => dto.ShortDescription));

        CreateMap<ArticleDto, ArticleResponse>();
        CreateMap<CreateOrUpdateArticleRequest, ArticleDto> ();



    }
}