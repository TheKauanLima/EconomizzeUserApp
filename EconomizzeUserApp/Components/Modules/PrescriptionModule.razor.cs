using Blazored.Modal;
using Blazored.Modal.Services;
using Economizze.Library;
using EconomizzeUserApp.Components.Modal;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class PrescriptionModule
    {
        //prescriptions
        private List<PrescriptionModel> prescriptions = new();
        private List<PrescriptionImageModel> prescriptionImages = new();

        //prescription images
        private PrescriptionModel newPrescription = InitializeNewPrescription();
        private PrescriptionImageModel newPrescriptionImage = new();
        private IBrowserFile? selectedFile;
        private string uploadedFileName = string.Empty;
        private bool isFileSelected = false;

        // State flags for submission process
        private bool isSubmitting = false;
        private bool isSubmitted = false;

        private static int UserId;

        [CascadingParameter] IModalService? Modal { get; set; }

        #region LIFECYCLE METHODS
        // set necessary values
        protected override void OnInitialized()
        {
            UserId = UserLoginService.CurrentEntity!.UserId;
            /* PrescriptionService.SetListValues(); */
        }
        #endregion

        #region PRESCRIPTION HANDLERS
        // set prescription default values
        private static PrescriptionModel InitializeNewPrescription() => new()
        {
            PrescriptionUnique = Guid.NewGuid(),
            CreatedBy = UserId,
            CreatedOn = DateTime.Now,
            ModifiedBy = UserId,
            ModifiedOn = DateTime.Now
        };

        // clone prescriptionImage to an object (to avoid referencing it itself)
        private PrescriptionImageModel ClonePrescriptionImage() => new()
        {
            PrescriptionUnique = newPrescription.PrescriptionUnique,
            FileExtension = newPrescriptionImage.FileExtension,
            ImageData = newPrescriptionImage.ImageData,
            Base64Image = newPrescriptionImage.Base64Image
        };

        // add prescription and image data to the list
        private void SavePrescription()
        {
            if (selectedFile == null) return; // ensure file is selected

            var clonedImage = ClonePrescriptionImage();
            prescriptions.Add(newPrescription);
            prescriptionImages.Add(clonedImage);

            ResetForm();
        }

        // reset models and variables to default values
        private void ResetForm()
        {
            newPrescription = InitializeNewPrescription();
            newPrescriptionImage = new();
            uploadedFileName = string.Empty;
            isFileSelected = false;
        }

        // submit all files to cloud
        private async Task SubmitFiles()
        {
            if (isSubmitting || isSubmitted) return; // prevent multiple submissions

            isSubmitting = true;

            try
            {
                foreach (var prescription in prescriptions.ToList()) // add each prescription
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

        // set prescription id to image prescription
        private void UpdateImagePrescriptionId(PrescriptionModel prescription)
        {
            foreach (var image in prescriptionImages.Where(img => img.PrescriptionUnique == prescription.PrescriptionUnique))
            {
                image.PrescriptionId = prescription.PrescriptionId;
            }
        }

        // add prescription to service
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

        // save prescription image to cloud
        private async Task SavePrescriptionImages(PrescriptionModel prescription)
        {
            var imagesCopy = prescriptionImages.Where(img => img.PrescriptionId == prescription.PrescriptionId).ToList();
            foreach (var image in imagesCopy)
            {
                // give image correct url and upload
                var objectName = $"{prescription.PrescriptionUnique}{image.FileExtension}";
                var publicUrl = await StorageService.UploadFileAsync(image.ImageData!, objectName);
                image.ImageUrl = publicUrl;

                var pi = Mapper.Map<PrescriptionImage>(image);
                PrescriptionService.PrescriptionImages.Add(pi);
            }
        }
        #endregion

        #region IMAGE HANDLING
        // handle file properties when chosen
        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            try
            {
                selectedFile = e.File; // hold file data
                uploadedFileName = selectedFile.Name;
                newPrescriptionImage.FileExtension = Path.GetExtension(uploadedFileName);

                // offload file processing to a background task
                await Task.Run(async () =>
                {
                    
                    newPrescriptionImage.ImageData = // write image into bytes to upload
                        await ConvertToByteArrayAsync(selectedFile);

                    newPrescriptionImage.Base64Image = // write bytes to string to display
                        Convert.ToBase64String(newPrescriptionImage.ImageData);
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

        // convert image to an array of bytes
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
        // pop up modal window with bigger image
        private async Task ShowImageModal(string image)
        {
            var parameters = new ModalParameters { { "imagem", image } };
            var imageModal = Modal!.Show<ImageModal>("Prescricao seletada", parameters);
            await imageModal.Result;
        }

        // finish the quote and navigate back to the quote page
        private void FinishQuote()
        {
            if (isSubmitted)
            {
                Navigation.NavigateTo("/orcamento");
            }
        }

        // delete element in prescriptions lists
        private void DeletePrescription(Guid prescriptionUnique)
        {
            prescriptions.RemoveAll(p => p.PrescriptionUnique == prescriptionUnique);
            prescriptionImages.RemoveAll(img => img.PrescriptionUnique == prescriptionUnique);
        }
        #endregion
    }
}