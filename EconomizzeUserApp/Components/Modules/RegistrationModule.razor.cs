using AutoMapper;
using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class RegistrationModule
    {
        [SupplyParameterFromForm(FormName = "Register")]
        private RegisterModel register { get; set; } = new();

        private String message = String.Empty;
        private bool isVisible = false;
        private string? UserUniqueId { get; set; }
        private bool NewUser = false;
        private bool HideRegistration = false;

        #region INITIALIZE
        protected override void OnInitialized()
        {
            StatusHandler.Message = string.Empty;
            UserLoginService.SetListValues();
        }
        #endregion


        private async Task RegisterUserAsync()
        {
            ArgumentNullException.ThrowIfNull(register);

            register.UserUniqueId = Guid.NewGuid();

            var map = Mapper.Map<UserLogin>(register);
            var userLogin = await UserLoginService.CreateAsync(map);
            register = Mapper.Map<RegisterModel>(userLogin);

            isVisible = true;
            NewUser = true;

            UserUniqueId = register.UserUniqueId.ToString();
            message = StatusHandler.Message;
            HideRegistration = true;
        }

        private async Task Verify()
        {
            if (NewUser)
            {
                ArgumentNullException.ThrowIfNull(register);
                var map = Mapper.Map<UserLogin>(register);
                await UserLoginService.VerifyAsync(map);
                message = StatusHandler.Message;
            }
            NavigationManager.NavigateTo("login");

            NewUser = false;
        }
    }
}