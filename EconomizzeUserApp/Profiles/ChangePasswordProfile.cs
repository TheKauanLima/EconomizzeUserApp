using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class ChangePasswordProfile : Profile
    {
        public ChangePasswordProfile()
        {
            CreateMap<UserLogin, ChangePasswordModel>().ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => src.NewPassword));
        }
    }
}
