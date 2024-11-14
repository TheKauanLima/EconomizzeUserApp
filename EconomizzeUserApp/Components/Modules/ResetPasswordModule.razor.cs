using Economizze.Library;
using EconomizzeUserApp.Model;
using EconomizzeUserApp.Services.Classes.Handler;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to handle password reset functionality for users.
    /// Allows users to reset their password and provides feedback on the reset status.
    /// </summary>
    public partial class ResetPasswordModule : IDisposable
    {
        /// <summary>
        /// User ID passed as a parameter from the URL or parent component.
        /// </summary>
        [Parameter] public int UserId { get; set; }

        /// <summary>
        /// Unique identifier for the user, passed as a parameter.
        /// </summary>
        [Parameter] public Guid UserUniqueId { get; set; }

        /// <summary>
        /// Model that holds the password reset details, including the new password.
        /// </summary>
        private ForgotPasswordModel changePasswordModel = new();

        /// <summary>
        /// Indicates whether the password has been successfully changed.
        /// </summary>
        private bool _passwordChanged = false;

        /// <summary>
        /// Text used to display a redirecting message with an ellipsis animation.
        /// </summary>
        private string redirectingText = string.Empty;

        /// <summary>
        /// Timer used to control the ellipsis animation for the redirecting message.
        /// </summary>
        private System.Timers.Timer? _timer;

        /// <summary>
        /// Counter to manage the number of dots displayed in the ellipsis animation.
        /// </summary>
        private int _dotCount = 0;

        #region LIFECYCLE METHODS

        /// <summary>
        /// Initializes the component by pre-filling the UserId, UserUniqueId, and Username.
        /// </summary>
        protected override void OnInitialized()
        {
            changePasswordModel.UserId = UserId;
            changePasswordModel.UserUniqueId = UserUniqueId;
            changePasswordModel.Username = UsernameHandler.Username;
        }
        #endregion

        #region PASSWORD RESET HANDLER

        /// <summary>
        /// Handles the password reset process by updating the user's password.
        /// If successful, starts a redirecting animation and navigates to the login page.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandlePasswordReset()
        {
            try
            {
                // Map the password reset model to a UserLogin entity
                var userEntity = Mapper.Map<UserLogin>(changePasswordModel);

                // Attempt to change the password
                bool result = await UserLoginService.ChangePasswordAsync(userEntity);

                if (result)
                {
                    _passwordChanged = true;

                    // Start the ellipsis animation and redirect after a delay
                    StartEllipsisAnimation();
                    await Task.Delay(5000);
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    Console.WriteLine("Password reset failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password reset: {ex.Message}");
            }
        }
        #endregion

        #region ELLIPSIS ANIMATION

        /// <summary>
        /// Starts the ellipsis animation using a timer to update the display every 500ms.
        /// </summary>
        private void StartEllipsisAnimation()
        {
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += (sender, args) => UpdateEllipsis();
            _timer.Start();
        }

        /// <summary>
        /// Updates the ellipsis display by cycling through dots.
        /// Invokes StateHasChanged to refresh the UI.
        /// </summary>
        private void UpdateEllipsis()
        {
            _dotCount = (_dotCount + 1) % 4;
            redirectingText = new string('.', _dotCount);
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region DISPOSAL

        /// <summary>
        /// Disposes the timer to release resources when the component is destroyed.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }
        #endregion
    }
}
