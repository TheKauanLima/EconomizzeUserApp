using Economizze.Library;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IStreetDetailViewService : IService<StreetDetailView>
    {
        Task<StreetDetailView?> ReadStreetDetailByIdAsync(int streetId);
        Task<StreetDetailView> ReadStreetDetailByZipCodeAsync(string zipCode);
    }
}
