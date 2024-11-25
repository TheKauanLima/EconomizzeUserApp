using Blazored.Modal;
using Blazored.Modal.Services;
using Economizze.Library;
using EconomizzeUserApp.Components.Modal;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.IO.Compression;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to manage prescription data, including text and images, 
    /// with file handling and modal interactions.
    /// </summary>
    public partial class PrescriptionModule
    {
        // List of prescriptions, images, and texts.
        private List<PrescriptionModel> prescriptions = new();
        private List<PrescriptionImageModel> prescriptionImages = new();
        private List<PrescriptionTextModel> prescriptionTexts = new();

        // Current prescription and related models.
        private PrescriptionModel newPrescription = InitializeNewPrescription();
        private PrescriptionImageModel newPrescriptionImage = new();
        private PrescriptionTextModel newPrescriptionText = new() { PrescriptionId = 0, Text = string.Empty };

        // File handling fields.
        private IBrowserFile? selectedFile;
        private string uploadedFileName = string.Empty;
        private bool isFileSelected = false;

        // Submission state tracking.
        private bool isSubmitting = false;
        private bool isSubmitted = false;

        // Current user ID for logged-in user.
        private static int UserId;

        // Modal service for displaying modals.
        [CascadingParameter]
        IModalService? Modal { get; set; }

        #region LIFECYCLE METHODS

        /// <summary>
        /// Initializes the component and sets the UserId.
        /// </summary>
        protected override void OnInitialized()
        {
            UserId = UserLoginService.CurrentEntity!.UserId;
        }

        #endregion

        #region PRESCRIPTION HANDLER

        /// <summary>
        /// Creates a new prescription with initialized default values.
        /// </summary>
        private static PrescriptionModel InitializeNewPrescription() => new()
        {
            PrescriptionUnique = Guid.NewGuid(),
            CreatedBy = UserId,
            CreatedOn = DateTime.Now,
            ModifiedBy = UserId,
            ModifiedOn = DateTime.Now
        };

        #endregion

        #region CLONE PRESCRIPTIONS

        /// <summary>
        /// Clones the current prescription image model for saving.
        /// </summary>
        private PrescriptionImageModel ClonePrescriptionImage() => new()
        {
            PrescriptionUnique = newPrescription.PrescriptionUnique,
            FileExtension = newPrescriptionImage.FileExtension,
            ImageData = newPrescriptionImage.ImageData,
            Base64Image = newPrescriptionImage.Base64Image
        };

        /// <summary>
        /// Clones the current prescription text model for saving.
        /// </summary>
        private PrescriptionTextModel ClonePrescriptionText() => new()
        {
            PrescriptionUnique = newPrescription.PrescriptionUnique,
            Text = newPrescriptionText.Text
        };

        #endregion

        #region CREATE PRESCRIPTIONS

        /// <summary>
        /// Saves the current prescription along with its associated image.
        /// </summary>
        private void SavePrescription()
        {
            if (selectedFile == null) return;

            var clonedImage = ClonePrescriptionImage();
            prescriptions.Add(newPrescription);
            prescriptionImages.Add(clonedImage);
            ResetForm();
        }

        /// <summary>
        /// Saves the current prescription text.
        /// </summary>
        private void SavePrescriptionText()
        {
            if (string.IsNullOrEmpty(newPrescriptionText.Text)) return;

            var clonedText = ClonePrescriptionText();
            prescriptions.Add(newPrescription);
            prescriptionTexts.Add(clonedText);
            ResetTextForm();
        }

        #endregion

        #region FORM RESETS

        /// <summary>
        /// Resets the form to prepare for a new prescription entry.
        /// </summary>
        private void ResetForm()
        {
            newPrescription = InitializeNewPrescription();
            newPrescriptionImage = new();
            uploadedFileName = string.Empty;
        }

        /// <summary>
        /// Resets the text form for a new prescription text entry.
        /// </summary>
        private void ResetTextForm()
        {
            newPrescription = InitializeNewPrescription();
            newPrescriptionText = new();
        }

        #endregion

        #region SUBMISSION

        /// <summary>
        /// Submits the prescription data and associated images or texts.
        /// </summary>
        private async Task SubmitPrescription()
        {
            if (isSubmitting || isSubmitted) return;

            isSubmitting = true;

            try
            {
                foreach (var prescription in prescriptions.ToList())
                {
                    await AddPrescription(prescription);

                    if (prescriptionImages.Any(img => img.PrescriptionUnique == prescription.PrescriptionUnique))
                    {
                        UpdateImagePrescriptionId(prescription);
                        await SavePrescriptionImages(prescription);
                    }
                    if (prescriptionTexts.Any(txt => txt.PrescriptionUnique == prescription.PrescriptionUnique))
                    {
                        UpdateTextPrescriptionId(prescription);
                        SavePrescriptionTexts(prescription);
                    }
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
                isFileSelected = false;
            }
        }

        /// <summary>
        /// Adds a prescription to the database and updates its ID.
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

        #endregion

        #region UPDATE PRESCRIPTION IDS

        /// <summary>
        /// Updates the prescription ID for associated images.
        /// </summary>
        private void UpdateImagePrescriptionId(PrescriptionModel prescription)
        {
            foreach (var image in prescriptionImages.Where(img => img.PrescriptionUnique == prescription.PrescriptionUnique))
            {
                image.PrescriptionId = prescription.PrescriptionId;
            }
        }

        /// <summary>
        /// Updates the prescription ID for associated texts.
        /// </summary>
        private void UpdateTextPrescriptionId(PrescriptionModel prescription)
        {
            foreach (var text in prescriptionTexts.Where(txt => txt.PrescriptionUnique == prescription.PrescriptionUnique))
            {
                text.PrescriptionId = prescription.PrescriptionId;
            }
        }

        #endregion

        #region SAVE PRESCRIPTION LISTS

        /// <summary>
        /// Saves all images associated with a prescription to the database.
        /// </summary>
        private async Task SavePrescriptionImages(PrescriptionModel prescription)
        {
            var imagesCopy = prescriptionImages.Where(img => img.PrescriptionId == prescription.PrescriptionId).ToList();
            foreach (var image in imagesCopy)
            {
                var objectName = $"{prescription.PrescriptionUnique}{image.FileExtension}.gz";
                var publicUrl = await StorageService.UploadFileAsync(image.ImageData!, objectName);
                image.ImageUrl = publicUrl;

                var pi = Mapper.Map<PrescriptionImage>(image);
                PrescriptionService.PrescriptionImages.Add(pi);
            }
        }

        /// <summary>
        /// Saves all texts associated with a prescription to the database.
        /// </summary>
        private void SavePrescriptionTexts(PrescriptionModel prescription)
        {
            var textsCopy = prescriptionTexts.Where(txt => txt.PrescriptionId == prescription.PrescriptionId).ToList();
            foreach (var text in textsCopy)
            {
                var pt = Mapper.Map<PrescriptionText>(text);
                PrescriptionService.PrescriptionTexts.Add(pt);
            }
        }

        #endregion

        #region IMAGE HANDLING

        /// <summary>
        /// Handles file selection and processes the selected file.
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
                    newPrescriptionImage.ImageData = CompressFile(newPrescriptionImage.ImageData);
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
        /// Converts the selected file to a byte array for processing.
        /// </summary>
        private static async Task<byte[]> ConvertToByteArrayAsync(IBrowserFile browserFile)
        {
            const long maxFileSize = 10 * 1024 * 1024; // 10 MB limit
            await using var stream = browserFile.OpenReadStream(maxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Compresses the given byte array using GZip compression.
        /// </summary>
        private static byte[] CompressFile(byte[] fileData)
        {
            using var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outputStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(fileData, 0, fileData.Length);
            }
            return outputStream.ToArray();
        }

        #endregion

        #region MODAL

        /// <summary>
        /// Displays a modal with the selected prescription image.
        /// </summary>
        private async Task ShowImageModal(string image)
        {
            var parameters = new ModalParameters { { "imageString", image } };
            var imageModal = Modal!.Show<ImageModal>("Prescricao seletada", parameters);
            await imageModal.Result;
        }

        #endregion

        #region NAVIGATION

        /// <summary>
        /// Navigates to the quote page after submission.
        /// </summary>
        private void FinishQuote()
        {
            if (isSubmitted)
            {
                Navigation.NavigateTo("/orcamento");
            }
        }

        #endregion

        #region LIST METHODS

        /// <summary>
        /// Deletes a prescription and its associated data by unique ID.
        /// </summary>
        private void DeletePrescription(Guid prescriptionUnique)
        {
            prescriptions.RemoveAll(p => p.PrescriptionUnique == prescriptionUnique);
            prescriptionImages.RemoveAll(img => img.PrescriptionUnique == prescriptionUnique);
            prescriptionTexts.RemoveAll(txt => txt.PrescriptionUnique == prescriptionUnique);
        }

        #endregion
    }
}
