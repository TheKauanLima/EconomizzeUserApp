using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to handle user registration functionality.
    /// Allows users to register and verify their accounts.
    /// </summary>
    public partial class RegistrationModule
    {
        /// <summary>
        /// Model to capture the registration details from the form.
        /// </summary>
        [SupplyParameterFromForm(FormName = "Register")]
        private RegisterModel register { get; set; } = new();

        /// <summary>
        /// Message to display feedback to the user after registration or verification.
        /// </summary>
        private string message = string.Empty;

        /// <summary>
        /// Indicates whether the registration status message is visible.
        /// </summary>
        private bool isVisible = false;

        /// <summary>
        /// Stores the unique ID of the newly registered user.
        /// </summary>
        private string? UserUniqueId { get; set; }

        /// <summary>
        /// Flag indicating whether a new user has been registered.
        /// </summary>
        private bool NewUser = false;

        /// <summary>
        /// Controls the visibility of the registration form after successful registration.
        /// </summary>
        private bool HideRegistration = false;

        #region INITIALIZATION

        /// <summary>
        /// Initializes the component by setting default values and loading necessary data.
        /// </summary>
        protected override void OnInitialized()
        {
            // Clear any previous status messages
            StatusHandler.Message = string.Empty;

            // Ensure the UserLoginService list is initialized
            UserLoginService.SetListValues();
        }
        #endregion

        #region USER REGISTRATION

        /// <summary>
        /// Registers a new user asynchronously.
        /// Maps the registration model to the UserLogin entity and saves it to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RegisterUserAsync()
        {
            // Ensure the registration model is not null
            ArgumentNullException.ThrowIfNull(register);

            try
            {
                // Generate a unique ID for the new user
                register.UserUniqueId = Guid.NewGuid();

                // Map the RegisterModel to UserLogin and create a new user
                var userEntity = Mapper.Map<UserLogin>(register);
                var createdUser = await UserLoginService.CreateAsync(userEntity);

                // Update the register model with the created user details
                register = Mapper.Map<RegisterModel>(createdUser);

                // Update UI states
                isVisible = true;
                NewUser = true;
                UserUniqueId = register.UserUniqueId.ToString();
                message = StatusHandler.Message;
                HideRegistration = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex}");
                message = $"Error: {ex.Message}";
            }
        }
        #endregion

        #region USER VERIFICATION

        /// <summary>
        /// Verifies the newly registered user asynchronously.
        /// Maps the registration model to the UserLogin entity and performs verification.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task Verify()
        {
            if (NewUser)
            {
                // Ensure the registration model is not null
                ArgumentNullException.ThrowIfNull(register);

                try
                {
                    // Map the RegisterModel to UserLogin and verify the user
                    var userEntity = Mapper.Map<UserLogin>(register);
                    await UserLoginService.VerifyAsync(userEntity);

                    // Display status message to the user
                    message = StatusHandler.Message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error verifying user: {ex}");
                    message = $"Error: {ex.Message}";
                }
            }

            // Navigate to the login page after verification
            NavigationManager.NavigateTo("login");
            NewUser = false;
        }
        #endregion
    }
}
