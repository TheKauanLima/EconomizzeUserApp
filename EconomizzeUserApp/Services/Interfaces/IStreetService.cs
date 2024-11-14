using Economizze.Library;
using EconomizzeUserApp.Services.Interfaces.Generic;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IStreetService : IService<Street>
    {
        Task<Street?> ReadByZipCodeAsync(string zipCode);
    }
}
