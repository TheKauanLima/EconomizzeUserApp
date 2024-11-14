using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to handle the change password functionality for users.
    /// Allows users to change their password after verifying their current password.
    /// </summary>
    public partial class ChangePasswordModule
    {
        /// <summary>
        /// Model holding the user's password change details, including current and new passwords.
        /// </summary>
        private ChangePasswordModel _changePasswordModel = new ChangePasswordModel();

        /// <summary>
        /// Flag indicating whether the password was successfully changed.
        /// </summary>
        private bool _passwordChanged = false;

        /// <summary>
        /// Stores error messages to be displayed to the user in case of an issue.
        /// </summary>
        private string _errorMessage = string.Empty;

        #region LIFECYCLE METHODS

        /// <summary>
        /// Lifecycle method called when the component is initialized.
        /// Sets the current user's ID and Unique ID from the application's settings.
        /// </summary>
        protected override void OnInitialized()
        {
            // Set user identifiers for the password change operation
            _changePasswordModel.UserId = SettingsService.appSettings.UserId;
            _changePasswordModel.UserUniqueId = SettingsService.appSettings.UserUniqueId;
        }
        #endregion

        #region CHANGE PASSWORD

        /// <summary>
        /// Handles the password change process.
        /// Validates the current password, updates the password if valid, and displays appropriate messages.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleChangePassword()
        {
            // Ensure the current password is not empty
            if (string.IsNullOrWhiteSpace(_changePasswordModel.CurrentPassword))
            {
                _errorMessage = "Por favor, insira sua senha atual.";
                return;
            }

            try
            {
                // Check if the entered current password matches the stored password hash
                if (_changePasswordModel.CurrentPassword != UserLoginService.CurrentEntity?.PasswordHash)
                {
                    _errorMessage = "Senha atual incorreta.";
                    return;
                }

                // Map the change password model to the UserLogin entity
                var changePassword = Mapper.Map<UserLogin>(_changePasswordModel);

                // Attempt to change the password via the service
                var isChanged = await UserLoginService.ChangePasswordAsync(changePassword);

                // Handle success or failure of password change
                if (isChanged)
                {
                    _passwordChanged = true;
                    _errorMessage = string.Empty; // Clear error message on success
                }
                else
                {
                    _errorMessage = "Erro ao trocar a senha. Tente novamente.";
                }
            }
            catch (Exception ex)
            {
                // Log the error and set an error message
                Console.WriteLine($"Error changing password: {ex}");
                _errorMessage = $"Ocorreu um erro: {ex.Message}";
            }
        }
        #endregion
    }
}
