namespace EconomizzeUserApp.Model
{
    public class PrescriptionTextModel
    {
        public long PrescriptionId { get; set; }
        public Guid PrescriptionUnique {  get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
