using Google.Cloud.Storage.V1;
using System.IO.Compression;

namespace EconomizzeUserApp.Services.Classes.Handler
{
    public static class CloudStorageHandler
    {
        public static string Base64String { get; set; } = String.Empty;

        /// <summary>
        /// Downloads and decompresses a Gzip file from Google Cloud Storage,
        /// then converts it to a Base64 string for display.
        /// </summary>
        /// <param name="bucketName">The name of the Google Cloud Storage bucket.</param>
        /// <param name="fileName">The name of the compressed file (e.g., "image.jpg.gz").</param>
        /// <returns>The Base64 string representing the decompressed image.</returns>
        public static async Task<string> GetBase64ImageFromGzFileAsync(string imageUrl)
        {
            string bucketName = "economizze_user_app_test_storage_bucket";
            string fileName = imageUrl.Substring(imageUrl.LastIndexOf('/') + 1);
            try
            {
                // Step 1: Download the compressed .gz file from Google Cloud Storage
                var compressedData = await DownloadFileFromCloudAsync(bucketName, fileName);

                // Step 2: Decompress the .gz file to retrieve the original image data
                var decompressedData = DecompressFile(compressedData);

                // Step 3: Convert the decompressed data to a Base64 string
                Base64String = Convert.ToBase64String(decompressedData);
                return Base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Base64 image from Gzip file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Downloads a file from Google Cloud Storage as a byte array.
        /// </summary>
        /// <param name="bucketName">The name of the Google Cloud Storage bucket.</param>
        /// <param name="fileName">The name of the file to download.</param>
        /// <returns>The file data as a byte array.</returns>
        private static async Task<byte[]> DownloadFileFromCloudAsync(string bucketName, string fileName)
        {
            try
            {
                // Move up from the base directory to the project root and then access the "resources" folder
                string keyFilePath = Path.Combine("C:/Development/EconomizzeUserApp/EconomizzeUserApp/Resources/economizzeuserapp-441700-628b4df09d34.json");

                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(keyFilePath);

                var storageClient = await StorageClient.CreateAsync(credential);

                using var memoryStream = new MemoryStream();
                await storageClient.DownloadObjectAsync(bucketName, fileName, memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file from Google Cloud: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Decompresses a Gzip-compressed byte array.
        /// </summary>
        /// <param name="compressedData">The compressed data as a byte array.</param>
        /// <returns>The decompressed data as a byte array.</returns>
        private static byte[] DecompressFile(byte[] compressedData)
        {
            try
            {
                using var inputStream = new MemoryStream(compressedData);
                using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
                using var outputStream = new MemoryStream();
                gzipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decompressing Gzip file: {ex.Message}");
                throw;
            }
        }
    }
}
