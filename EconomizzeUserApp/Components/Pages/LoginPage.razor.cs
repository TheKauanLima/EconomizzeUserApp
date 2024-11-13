using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Pages
{
    public partial class LoginPage
    {
        private UserLoginModel loginModel = new UserLoginModel();
        private UserLogin CurrentUser = new();
        private AppSettings AppSettings { get; set; } = new();

        #region INITIALIZE
        protected override void OnInitialized()
        {
            StatusHandler.Message = string.Empty;
            UserLoginService.SetListValues();
        }
        #endregion

        private async Task HandleLogin()
        {
            // Check if the login is successful
            var map = Mapper.Map<UserLogin>(loginModel);
            var isSuccess = await UserLoginService.AuthenticateUserAsync(map);

            if (isSuccess)
            {
                CurrentUser = UserLoginService.CurrentEntity!;
                await CreateSession();
            }
        }

        private async Task CreateSession()
        {
            await Task.Delay(0);
            if (UserLoginService.CurrentEntity is not null)
            {
                NavigationManager.NavigateTo("settings");
            }
        }

        private async Task GoToRegisterModule()
        {
            await Task.Delay(0);
            NavigationManager.NavigateTo("Criar");
        }

        private void GoToForgotPasswordModule()
        {
            NavigationManager.NavigateTo("/forgot-password");
        }
    }
}