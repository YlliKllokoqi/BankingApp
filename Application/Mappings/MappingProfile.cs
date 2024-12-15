using AutoMapper;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Mappings;

public class MappingProfile :Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, LoginDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
        CreateMap<ApplicationUser, UpdateDto>().ReverseMap();
    }
}