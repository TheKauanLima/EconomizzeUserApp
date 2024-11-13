using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Profiles
{
    public class PrescriptionProfile : Profile
    {
        public PrescriptionProfile()
        {
            CreateMap<Prescription, PrescriptionModel>().ReverseMap();
        }
    }
}
