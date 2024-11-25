using Economizze.Library;
using EconomizzeUserApp.Services.Interfaces.Generic;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IPrescriptionService : IService<Prescription>
    {
        List<PrescriptionImage> PrescriptionImages { get; set; }
        List<PrescriptionText> PrescriptionTexts { get; set; }
        Task<List<Prescription>> GetByQuoteIdAsync(int quoteId);
        Task<List<PrescriptionImage>> GetImagesByPrescriptionIdAsync(int prescriptionId);
        Task<List<PrescriptionImage>> GetAllPrescriptionImagesAsync();
        Task<List<PrescriptionText>> GetAllPrescriptionTextsAsync();
    }
}
