namespace EconomizzeUserApp.Model
{
    public class StreetDetailViewModel
    {
        public int StreetId { get; set; }
        public int NeighborhoodId { get; set; }
        public string? StreetName { get; set; } = string.Empty;
        public string? StreetNameAscii { get; set; } = string.Empty;
        public string? Zipcode { get; set; } = string.Empty;
        public string? NeighborhoodName { get; set; } = string.Empty;
        public string? CityName { get; set; } = string.Empty;
        public string? StateName { get; set; } = string.Empty;
    }
}
