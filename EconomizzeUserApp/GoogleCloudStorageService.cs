using Google.Cloud.Storage.V1;

public class GoogleCloudStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleCloudStorageService(string bucketName, string serviceAccountPath)
    {
        _bucketName = bucketName;
        _storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential
                        .FromFile(serviceAccountPath));
    }

    // Uploads a file to Google Cloud Storage using a byte array
    public async Task<string> UploadFileAsync(byte[] fileData, string objectName)
    {
        using var memoryStream = new MemoryStream(fileData);
        var storageObject = await _storageClient.UploadObjectAsync(_bucketName, objectName, null, memoryStream);

        // Return the public URL of the uploaded file
        return $"https://storage.googleapis.com/{_bucketName}/{storageObject.Name}";
    }

    // Download a file from GCS
    public async Task DownloadFileAsync(string objectName, string destinationPath)
    {
        using var outputStream = File.OpenWrite(destinationPath);
        await _storageClient.DownloadObjectAsync(_bucketName, objectName, outputStream);
    }

    // Delete a file from GCS
    public async Task DeleteFileAsync(string objectName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, objectName);
    }
}
