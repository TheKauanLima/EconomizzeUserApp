using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class UserDetailProfile : Profile
    {
        public UserDetailProfile()
        {
            CreateMap<User, UserDetailModel>().ReverseMap();
        }
    }
}
