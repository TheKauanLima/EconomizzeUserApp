namespace EconomizzeUserApp.Services.Classes.Handler
{
    public static class CloudStorageHandler
    {
        /// <summary>
        /// Generates a full cloud path for the given image URL if it is not an absolute URL.
        /// </summary>
        /// <param name="imageUrl">The image URL to validate.</param>
        /// <returns>A valid cloud storage path or an empty string if the URL is invalid.</returns>
        public static string GetCloudImagePath(string? imageUrl)
        {
            // ensure image url is present
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                Console.WriteLine("Invalid Image URL provided.");
                return string.Empty;
            }

            // return the full cloud path if necessary
            return imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? imageUrl
                : $"https://storage.googleapis.com/your-bucket-name/{imageUrl}";
        }
    }
}
