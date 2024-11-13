namespace EconomizzeUserApp.Components.Pages
{
    public partial class SettingsPage
    {
        private AppSettings? _settings = new AppSettings(); // Initialize with a default instance
        private string? _userUniqueIdString;

        protected override async Task OnInitializedAsync()
        {
            await SetAppSettings();
        }

        public async Task SaveSettings()
        {
            if (Guid.TryParse(_userUniqueIdString, out var userUniqueId))
            {
                _settings!.UserUniqueId = userUniqueId;
            }

            await SettingsService.SaveSettingsAsync(_settings!);
            Navigation.NavigateTo("/");
        }

        public async Task SetAppSettings()
        {
            _settings = await SettingsService.LoadSettingsAsync();
            _settings.UserId = UserLoginService.CurrentEntity!.UserId;
            _settings.UserUniqueId = UserLoginService.CurrentEntity!.UserUniqueId;
            _settings.Username = UserLoginService.CurrentEntity!.Username;
            _settings.StartDate = UserLoginService.CurrentEntity!.CreatedOn;

            _userUniqueIdString = _settings.UserUniqueId.ToString();
            UpdateEndDate(); // Set initial end date when loading settings
            await SaveSettings();
        }

        private void UpdateEndDate()
        {
            // Set End Date to one year after Start Date
            if (_settings?.StartDate.HasValue == true)
            {
                _settings.EndDate = _settings.StartDate.Value.AddYears(1);
            }
        }
    }
}