namespace EconomizzeUserApp.Model
{
    public class PrescriptionModel
    {
        public long PrescriptionId { get; set; }
        public int QuoteId { get; set; }
        public Guid PrescriptionUnique { get; set; }
        public int FacilityId { get; set; }
        public int ProfessionalId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
