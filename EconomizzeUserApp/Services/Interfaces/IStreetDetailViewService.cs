using Economizze.Library;
using EconomizzeUserApp.Services.Interfaces.Generic;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IStreetDetailViewService : IService<StreetDetailView>
    {
        Task<StreetDetailView?> ReadStreetDetailByIdAsync(int streetId);
        Task<StreetDetailView> ReadStreetDetailByZipCodeAsync(string zipCode);
    }
}
