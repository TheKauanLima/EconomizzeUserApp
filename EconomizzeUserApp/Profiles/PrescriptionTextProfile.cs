using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class PrescriptionTextProfile : Profile
    {
        public PrescriptionTextProfile()
        {
            CreateMap<PrescriptionText, PrescriptionTextModel>().ReverseMap();
        }
    }
}
