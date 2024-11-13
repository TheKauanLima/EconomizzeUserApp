namespace EconomizzeUserApp
{
    public class AppSettings
    {
        public int UserId { get; set; }
        public Guid UserUniqueId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
        public DateTime VersionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
