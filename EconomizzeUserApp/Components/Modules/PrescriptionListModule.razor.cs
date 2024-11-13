using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;
using System.IO;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class PrescriptionListModule
    {
        [Parameter] public int QuoteId { get; set; }

        private List<PrescriptionModel> prescriptions = new();
        private List<PrescriptionImage> prescriptionImages = new();

        protected override async Task OnInitializedAsync()
        {
            if (PrescriptionService.CurrentEntity is null)
            { 
                PrescriptionService.SetListValues();
            }
            await LoadPrescriptionsForQuote();
        }

        private async Task LoadPrescriptionsForQuote()
        {
            // Fetch prescriptions for the given QuoteId
            var prescriptionEntities = await PrescriptionService.GetByQuoteIdAsync(QuoteId);
            prescriptions = Mapper.Map<List<PrescriptionModel>>(prescriptionEntities);

            // Fetch all images associated with the prescriptions
            prescriptionImages = await PrescriptionService.GetAllPrescriptionImagesAsync();
        }

        public string GetLocalImagePath(string imageUrl)
        {
            if (imageUrl != "")
            {
                var idkanymore = $"file:///{imageUrl.Replace("\\", "/")}";
                return idkanymore;
            }
            else
            {
                Console.WriteLine("File not found: " + imageUrl);
                return string.Empty;
            }
        }

        //private string GetLocalImagePath(string imageUrl)
        //{
        //    // Convert Windows path to file URL format
        //    string formattedPath = $"file:///{imageUrl.Replace("\\", "/")}";
        //    Console.WriteLine($"Formatted Image Path: {formattedPath}");
        //    return formattedPath;
        //}


        private async Task ShowImageModal(string imageUrl)
        {
            // Display the image in a modal or handle click event
            await Task.CompletedTask;
        }

        private void BackToQuotes()
        {
            Navigation.NavigateTo("/orcamento");
        }
    }
}