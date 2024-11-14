using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class PrescriptionListModule
    {
        #region Parameters
        [Parameter] public int QuoteId { get; set; }
        #endregion

        #region Fields
        private List<PrescriptionModel> prescriptions = new();
        private List<PrescriptionImage> prescriptionImages = new();
        #endregion

        #region Lifecycle Methods
        protected override async Task OnInitializedAsync()
        {
            await InitializePrescriptionData();
        }
        #endregion

        #region Initialization Methods
        private async Task InitializePrescriptionData()
        {
            if (PrescriptionService.CurrentEntity == null)
            {
                PrescriptionService.SetListValues();
            }

            await LoadPrescriptionsForQuoteAsync();
        }

        private async Task LoadPrescriptionsForQuoteAsync()
        {
            try
            {
                // Fetch prescriptions and map them to the model
                var prescriptionEntities = await PrescriptionService.GetByQuoteIdAsync(QuoteId);
                prescriptions = Mapper.Map<List<PrescriptionModel>>(prescriptionEntities);

                // Fetch all images associated with the prescriptions
                prescriptionImages = await PrescriptionService.GetAllPrescriptionImagesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading prescriptions: {ex.Message}");
            }
        }
        #endregion

        #region Helper Methods
        private string GetCloudImagePath(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                Console.WriteLine("Invalid Image URL provided.");
                return string.Empty;
            }

            return imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? imageUrl
                : $"https://storage.googleapis.com/your-bucket-name/{imageUrl}";
        }
        #endregion

        #region Modal Methods
        private async Task ShowImageModalAsync(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                Console.WriteLine("Cannot display modal for an empty image URL.");
                return;
            }

            var cloudImageUrl = GetCloudImagePath(imageUrl);
            await DisplayModal(cloudImageUrl);
        }

        private Task DisplayModal(string imageUrl)
        {
            // Placeholder for actual modal implementation
            Console.WriteLine($"Displaying modal for image: {imageUrl}");
            return Task.CompletedTask;
        }
        #endregion

        #region Navigation Methods
        private void NavigateBackToQuotes()
        {
            Navigation.NavigateTo("/orcamento");
        }
        #endregion
    }
}
