using System.Text.Json;


namespace EconomizzeUserApp
{
    public class SettingsService
    {
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Settings.json");
        public AppSettings appSettings;

        public SettingsService()
        {
            // Constructor for potential dependency injection or future setup if needed
            appSettings = new AppSettings();
        }

        /// <summary>
        /// Loads the application settings from a JSON file.
        /// </summary>
        /// <returns>AppSettings object with configuration values.</returns>
        public async Task<AppSettings> LoadSettingsAsync()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    appSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error loading settings: {ex.Message}");
                    appSettings = new AppSettings(); // Assign default settings for general error
                }
            }
            return appSettings;
        }

        /// <summary>
        /// Saves the application settings to a JSON file.
        /// </summary>
        /// <param name="settings">AppSettings object containing configuration values to save.</param>
        public async Task SaveSettingsAsync(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
