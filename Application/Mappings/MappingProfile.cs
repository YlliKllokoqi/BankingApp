using AutoMapper;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(Result<>), typeof(ResultDto<>)).ReverseMap();
        CreateMap<ErrorResponse, ErrorResponseDto>().ReverseMap();
        CreateMap<ApplicationUser, LoginDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
        CreateMap<ApplicationUser, UpdateDto>().ReverseMap();
        CreateMap<DebitCard, DebitCardDto>().ReverseMap();
    }
}