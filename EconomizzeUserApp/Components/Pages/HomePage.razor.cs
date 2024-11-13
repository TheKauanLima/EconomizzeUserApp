namespace EconomizzeUserApp.Components.Pages
{
    public partial class HomePage
    {
        private AppSettings? settings;
        protected override async Task OnInitializedAsync()
        {
            settings = SettingsService.appSettings;
            if (settings is null)
            {
                settings = await SettingsService.LoadSettingsAsync();
            }
            // Load the settings file asynchronously
            // await SettingsService.LoadSettingsAsync();
            await Task.Delay(1000);
            if (settings.UserId <= 0)
            {
                Navigation.NavigateTo("login");

            }
            else
            {
                Navigation.NavigateTo("dashboard");
            }

            // Redirect to the home page or main application page after loading
        }
    }
}