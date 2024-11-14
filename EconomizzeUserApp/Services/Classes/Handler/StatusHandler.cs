namespace EconomizzeUserApp.Services.Classes.Handler
{
    public class StatusHandler
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public StatusHandler()
        {
            Message = string.Empty;
            Error = false;
        }
    }
}
