using Economizze.Library;
using EconomizzeUserApp.Services.Classes;
using EconomizzeUserApp.Services.Interfaces;

namespace StoreApp.Services.Repositories
{
    public class PrescriptionService : BaseService<Prescription>, IPrescriptionService
    {
        public List<PrescriptionImage> PrescriptionImages { get; set; } = new List<PrescriptionImage>();
        public PrescriptionService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            Entities.AddRange(new List<Prescription>
            {
                new Prescription {
                    PrescriptionId = 1,
                    QuoteId = 1,
                    PrescriptionUnique = Guid.NewGuid(),
                    FacilityId = 0,
                    ProfessionalId = 0,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                }
            });

            PrescriptionImages.AddRange(new List<PrescriptionImage>
            {
                new PrescriptionImage
                {
                    PrescriptionId = 1,
                    ImageUrl = "C:/Development/EconomizzeUserApp/EconomizzeUserApp/Storage/test.jpg"
                }
            });
            CurrentEntity = Entities[^1];
        }
        #endregion

        #region CREATE
        public async Task<Prescription> CreateAsync(Prescription entity)
        {
            entity.PrescriptionId = Entities.Any() ? Entities.Max(p => p.PrescriptionId) + 1 : 1;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            Entities.Add(entity);
            CurrentEntity = entity;
            return await Task.FromResult(CurrentEntity!);
        }
        #endregion

        #region READ BY ID
        public async Task<Prescription?> ReadByIdAsync(object id)
        {
            if (id is int prescriptionId)
            {
                var result = Entities.FirstOrDefault(p => p.PrescriptionId == prescriptionId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<Prescription>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<Prescription?> UpdateAsync(Prescription entity)
        {
            var existingQuote = Entities.FirstOrDefault(p => p.PrescriptionId == entity.PrescriptionId);
            if (existingQuote != null)
            {
                return await Task.FromResult(existingQuote);
            }
            return null!;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int pid)
            {
                var quote = Entities.FirstOrDefault(p => p.PrescriptionId == pid);
                if (quote != null)
                {
                    Entities.Remove(quote);
                }
            }
            await Task.CompletedTask;
        }
        #endregion

        #region READ BY QUOTE ID
        public async Task<List<Prescription>> GetByQuoteIdAsync(int quoteId)
        {
            var prescriptions = Entities.Where(p => p.QuoteId == quoteId).ToList();
            return await Task.FromResult(prescriptions);
        }
        #endregion

        #region READ IMAGES BY PRESCRIPTION ID
        public async Task<List<PrescriptionImage>> GetImagesByPrescriptionIdAsync(int prescriptionId)
        {
            // Assuming you have another in-memory list to store PrescriptionImage entities
            if (PrescriptionImages == null) return new List<PrescriptionImage>();

            var images = PrescriptionImages.Where(img => img.PrescriptionId == prescriptionId).ToList();
            return await Task.FromResult(images);
        }
        #endregion

        public async Task<List<PrescriptionImage>> GetAllPrescriptionImagesAsync()
        {
            // Assuming PrescriptionImages is your in-memory list
            return await Task.FromResult(PrescriptionImages);
        }

    }
}
