using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class UserAddressProfile : Profile
    {
        public UserAddressProfile()
        {
            CreateMap<UserAddress, UserAddressModel>().ReverseMap();
        }
    }
}
