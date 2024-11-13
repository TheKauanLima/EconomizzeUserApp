using Economizze.Library;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IStreetService : IService<Street>
    {
        Task<Street?> ReadByZipCodeAsync(string zipCode);
    }
}
