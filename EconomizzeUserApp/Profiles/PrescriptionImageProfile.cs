using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class PrescriptionImageProfile : Profile
    {
        public PrescriptionImageProfile()
        {
            CreateMap<PrescriptionImage, PrescriptionImageModel>().ReverseMap();
        }
    }
}
