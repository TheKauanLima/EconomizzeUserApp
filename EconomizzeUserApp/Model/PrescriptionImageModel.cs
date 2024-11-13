namespace EconomizzeUserApp.Model
{
    public class PrescriptionImageModel
    {
        public long PrescriptionId { get; set; }
        public Guid PrescriptionUnique { get; set; }
        public string ImageUrl { get; set; } = string.Empty; // Store the file path
        public string FileExtension { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; } = null;
        public string Base64Image { get; set; } = string.Empty;
    }
}
