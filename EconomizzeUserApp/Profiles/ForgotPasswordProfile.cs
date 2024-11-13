using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<UserLogin, ForgotPasswordModel>().ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => src.NewPassword));
        }
    }
}
