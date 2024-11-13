using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class StreetDetailViewProfile : Profile
    {
        public StreetDetailViewProfile()
        {
            CreateMap<StreetDetailView, StreetDetailViewModel>().ReverseMap();
        }
    }
}
