using Blazored.Modal;
using Blazored.Modal.Services;
using Economizze.Library;
using EconomizzeUserApp.Components.Modal;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to manage prescriptions, including adding, saving, deleting, and uploading prescription files.
    /// </summary>
    public partial class PrescriptionModule
    {
        // List of prescriptions
        private List<PrescriptionModel> prescriptions = new();

        // List of prescription images
        private List<PrescriptionImageModel> prescriptionImages = new();

        // Models for new prescriptions and images
        private PrescriptionModel newPrescription = InitializeNewPrescription();
        private PrescriptionImageModel newPrescriptionImage = new();

        // File handling
        private IBrowserFile? selectedFile;
        private string uploadedFileName = string.Empty;
        private bool isFileSelected = false;

        // State flags for submission process
        private bool isSubmitting = false;
        private bool isSubmitted = false;

        // User ID for the current session
        private static int UserId;

        // Modal service to handle displaying modals
        [CascadingParameter] IModalService? Modal { get; set; }

        #region LIFECYCLE METHODS

        /// <summary>
        /// Initializes the component by setting the current user ID.
        /// </summary>
        protected override void OnInitialized()
        {
            UserId = UserLoginService.CurrentEntity!.UserId;
        }
        #endregion

        #region PRESCRIPTION HANDLERS

        /// <summary>
        /// Initializes a new PrescriptionModel with default values.
        /// </summary>
        /// <returns>A new PrescriptionModel instance.</returns>
        private static PrescriptionModel InitializeNewPrescription() => new()
        {
            PrescriptionUnique = Guid.NewGuid(),
            CreatedBy = UserId,
            CreatedOn = DateTime.Now,
            ModifiedBy = UserId,
            ModifiedOn = DateTime.Now
        };

        /// <summary>
        /// Creates a clone of the current PrescriptionImageModel to avoid reference issues.
        /// </summary>
        /// <returns>A new PrescriptionImageModel instance.</returns>
        private PrescriptionImageModel ClonePrescriptionImage() => new()
        {
            PrescriptionUnique = newPrescription.PrescriptionUnique,
            FileExtension = newPrescriptionImage.FileExtension,
            ImageData = newPrescriptionImage.ImageData,
            Base64Image = newPrescriptionImage.Base64Image
        };

        /// <summary>
        /// Saves the current prescription and its associated image to the list.
        /// </summary>
        private void SavePrescription()
        {
            if (selectedFile == null) return; // Ensure a file is selected

            var clonedImage = ClonePrescriptionImage();
            prescriptions.Add(newPrescription);
            prescriptionImages.Add(clonedImage);

            ResetForm();
        }

        /// <summary>
        /// Resets the form and clears all input fields.
        /// </summary>
        private void ResetForm()
        {
            newPrescription = InitializeNewPrescription();
            newPrescriptionImage = new();
            uploadedFileName = string.Empty;
            isFileSelected = false;
        }

        /// <summary>
        /// Submits all the prescriptions and their associated images to the cloud.
        /// </summary>
        private async Task SubmitFiles()
        {
            if (isSubmitting || isSubmitted) return;

            isSubmitting = true;

            try
            {
                // Submit each prescription and its images
                foreach (var prescription in prescriptions.ToList())
                {
                    await AddPrescription(prescription);
                    UpdateImagePrescriptionId(prescription);
                    await SavePrescriptionImages(prescription);
                }

                isSubmitted = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting files: {ex.Message}");
            }
            finally
            {
                isSubmitting = false;
            }
        }

        /// <summary>
        /// Updates the PrescriptionId for each associated image.
        /// </summary>
        private void UpdateImagePrescriptionId(PrescriptionModel prescription)
        {
            foreach (var image in prescriptionImages.Where(img => img.PrescriptionUnique == prescription.PrescriptionUnique))
            {
                image.PrescriptionId = prescription.PrescriptionId;
            }
        }

        /// <summary>
        /// Adds a new prescription to the service.
        /// </summary>
        private async Task AddPrescription(PrescriptionModel prescription)
        {
            prescription.QuoteId = QuoteService.CurrentEntity!.QuoteId;

            var result = await PrescriptionService.CreateAsync(
                Mapper.Map<Prescription>(prescription)
            );
            var updatedPrescription = Mapper.Map<PrescriptionModel>(result);

            var index = prescriptions.FindIndex(p => p.PrescriptionUnique == updatedPrescription.PrescriptionUnique);
            if (index != -1)
            {
                prescriptions[index].PrescriptionId = updatedPrescription.PrescriptionId;
            }
        }

        /// <summary>
        /// Saves prescription images to cloud storage.
        /// </summary>
        private async Task SavePrescriptionImages(PrescriptionModel prescription)
        {
            var imagesCopy = prescriptionImages.Where(img => img.PrescriptionId == prescription.PrescriptionId).ToList();
            foreach (var image in imagesCopy)
            {
                var objectName = $"{prescription.PrescriptionUnique}{image.FileExtension}";
                var publicUrl = await StorageService.UploadFileAsync(image.ImageData!, objectName);
                image.ImageUrl = publicUrl;

                var pi = Mapper.Map<PrescriptionImage>(image);
                PrescriptionService.PrescriptionImages.Add(pi);
            }
        }
        #endregion

        #region IMAGE HANDLING

        /// <summary>
        /// Handles the selection of a file from the input and processes it.
        /// </summary>
        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            try
            {
                selectedFile = e.File;
                uploadedFileName = selectedFile.Name;
                newPrescriptionImage.FileExtension = Path.GetExtension(uploadedFileName);

                await Task.Run(async () =>
                {
                    newPrescriptionImage.ImageData = await ConvertToByteArrayAsync(selectedFile);
                    newPrescriptionImage.Base64Image = Convert.ToBase64String(newPrescriptionImage.ImageData);
                });

                isFileSelected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling file selection: {ex.Message}");
                StatusHandler.Message = "Erro em processamento do arquivo.";
                uploadedFileName = string.Empty;
                isFileSelected = false;
            }
        }

        /// <summary>
        /// Converts an IBrowserFile to a byte array.
        /// </summary>
        private static async Task<byte[]> ConvertToByteArrayAsync(IBrowserFile browserFile)
        {
            const long maxFileSize = 10 * 1024 * 1024; // 10 MB
            await using var stream = browserFile.OpenReadStream(maxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion

        #region MODAL & NAVIGATION

        /// <summary>
        /// Displays a modal window with a larger view of the selected image.
        /// </summary>
        private async Task ShowImageModal(string image)
        {
            var parameters = new ModalParameters { { "imageString", image } };
            var imageModal = Modal!.Show<ImageModal>("Prescricao seletada", parameters);
            await imageModal.Result;
        }

        /// <summary>
        /// Completes the quote and navigates back to the quote page.
        /// </summary>
        private void FinishQuote()
        {
            if (isSubmitted)
            {
                Navigation.NavigateTo("/orcamento");
            }
        }

        /// <summary>
        /// Deletes a prescription from the list.
        /// </summary>
        private void DeletePrescription(Guid prescriptionUnique)
        {
            prescriptions.RemoveAll(p => p.PrescriptionUnique == prescriptionUnique);
            prescriptionImages.RemoveAll(img => img.PrescriptionUnique == prescriptionUnique);
        }
        #endregion
    }
}
