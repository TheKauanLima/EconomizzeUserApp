namespace EconomizzeUserApp.Model
{
    public class UserAddressModel
    {
        public int UserAddressId { get; set; }
        public int UserId { get; set; }
        public int? StreetId { get; set; }
        public string Complement { get; set; } = string.Empty;
        public string ComplementAscii { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
