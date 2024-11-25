using Blazored.Modal;
using Blazored.Modal.Services;
using Economizze.Library;
using EconomizzeUserApp.Components.Modal;
using EconomizzeUserApp.Model;
using EconomizzeUserApp.Services.Classes.Handler;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to manage and display a list of prescriptions related to a specific quote.
    /// Provides functionalities for fetching, viewing, and interacting with prescription data.
    /// </summary>
    public partial class PrescriptionListModule
    {
        /// <summary>
        /// The ID of the quote for which prescriptions are to be displayed.
        /// This value is passed as a parameter from the parent component.
        /// </summary>
        [Parameter] public int QuoteId { get; set; }

        /// <summary>
        /// List of all prescriptions associated with the specified QuoteId.
        /// </summary>
        private List<PrescriptionModel> prescriptions = new();

        /// <summary>
        /// List of all images associated with the prescriptions.
        /// </summary>
        private List<PrescriptionImageModel> prescriptionImages = new();

        /// <summary>
        /// List of all texts associated with the prescriptions.
        /// </summary>
        private List<PrescriptionTextModel> prescriptionTexts = new();

        /// <summary>
        /// Dictionary of all base 64 string.
        /// </summary>
        private Dictionary<string, string> base64Images = new();

        /// <summary>
        /// Modal service used to display modals (e.g., for showing prescription images).
        /// </summary>
        [CascadingParameter] IModalService? Modal { get; set; }

        #region LIFECYCLE METHODS

        /// <summary>
        /// Lifecycle method called when the component is initialized.
        /// Loads prescription data related to the provided QuoteId.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await InitializePrescriptionData();
            await PreloadBase64Images();
        }
        #endregion

        #region INITIALIZATION METHODS

        /// <summary>
        /// Initializes prescription data by setting up necessary values and loading prescriptions.
        /// </summary>
        private async Task InitializePrescriptionData()
        {
            // Ensure that the PrescriptionService is properly initialized
            if (PrescriptionService.CurrentEntity == null)
            {
                PrescriptionService.SetListValues();
            }

            // Load all prescriptions for the given QuoteId
            await LoadPrescriptionsForQuoteAsync();
        }

        /// <summary>
        /// Loads all prescriptions tied to the specified QuoteId.
        /// Fetches the data from the service and maps it to the model.
        /// </summary>
        private async Task LoadPrescriptionsForQuoteAsync()
        {
            try
            {
                // Fetch prescriptions from the service and map them to the PrescriptionModel
                var prescriptionEntities = await PrescriptionService.GetByQuoteIdAsync(QuoteId);
                prescriptions = Mapper.Map<List<PrescriptionModel>>(prescriptionEntities);

                // Fetch all images associated with the loaded prescriptions
                var pi = await PrescriptionService.GetAllPrescriptionImagesAsync();
                prescriptionImages = Mapper.Map<List<PrescriptionImageModel>>(pi);

                // Fetch all images associated with the loaded prescriptions
                var pt = await PrescriptionService.GetAllPrescriptionTextsAsync();
                prescriptionTexts = Mapper.Map<List<PrescriptionTextModel>>(pt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading prescriptions: {ex}");
            }
        }
        #endregion

        #region PRELOAD IMAGES
        private async Task PreloadBase64Images()
        {
            var tasks = new List<Task>();

            foreach (var prescriptionImage in prescriptionImages)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        // Process each image asynchronously
                        string base64Image = await CloudStorageHandler.GetBase64ImageFromGzFileAsync(prescriptionImage.ImageUrl);

                        // Ensure thread-safe access to the dictionary
                        lock (base64Images)
                        {
                            base64Images[prescriptionImage.ImageUrl] = base64Image;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading Base64 image for {prescriptionImage.ImageUrl}: {ex.Message}");
                        lock (base64Images)
                        {
                            base64Images[prescriptionImage.ImageUrl] = string.Empty; // Avoid nulls
                        }
                    }
                }));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }
        #endregion

        #region MODAL METHODS

        /// <summary>
        /// Displays a modal window with a larger view of the selected prescription image.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to display in the modal.</param>
        private async Task ShowImageModal(string imageUrl)
        {
            // Set up modal parameters to pass the image URL
            var parameters = new ModalParameters { { "imageUrl", imageUrl } };

            // Show the modal using Blazored Modal service
            var imageModal = Modal!.Show<ImageModal>("Prescrição selecionada", parameters);
            await imageModal.Result;
        }
        #endregion

        #region NAVIGATION METHODS

        /// <summary>
        /// Navigates back to the main quotes page.
        /// </summary>
        private void NavigateBackToQuotes()
        {
            Navigation.NavigateTo("/orcamento");
        }
        #endregion
    }
}
