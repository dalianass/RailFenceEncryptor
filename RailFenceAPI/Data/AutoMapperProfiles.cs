using AutoMapper;
using RailFenceAPI.DTOs;
using RailFenceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDto>();

            CreateMap<UserDto, AppUser>();
            //from, to
            CreateMap<RegisterDto, AppUser>();
            //CreateMap<AppUser, LoginDto>();

            CreateMap<Sifrovanje, SifrovanjeDto>().ReverseMap();

            CreateMap<Desifrovanje, DesifrovanjeDto>().ReverseMap();

            CreateMap<Sifrovanje, SvaSifrovanjaDto>().ReverseMap();

            CreateMap<Desifrovanje, SvaDesifrovanjaDto>().ReverseMap();
        }
    }
}
