using Economizze.Library;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IPrescriptionService : IService<Prescription>
    {
        public List<PrescriptionImage> PrescriptionImages { get; set; }
        Task<List<Prescription>> GetByQuoteIdAsync(int quoteId);
        Task<List<PrescriptionImage>> GetImagesByPrescriptionIdAsync(int prescriptionId);
        Task<List<PrescriptionImage>> GetAllPrescriptionImagesAsync();
    }
}
