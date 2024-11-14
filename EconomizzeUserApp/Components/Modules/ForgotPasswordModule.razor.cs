using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component to handle the "Forgot Password" functionality.
    /// Allows users to generate a password reset link based on their username.
    /// </summary>
    public partial class ForgotPasswordModule
    {
        /// <summary>
        /// Model to capture the user's input for username.
        /// </summary>
        private UsernameModel UsernameModel = new();

        /// <summary>
        /// Stores the generated password reset link if the user is found.
        /// </summary>
        private string resetLink = string.Empty;

        #region USER HANDLING

        /// <summary>
        /// Handles the "Forgot Password" process.
        /// Finds the user by username and generates a password reset link.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleForgotPassword()
        {
            // Ensure the username field is not empty
            if (string.IsNullOrWhiteSpace(UsernameModel.Username))
            {
                resetLink = string.Empty;
                return;
            }

            try
            {
                // Fetch all users from the service
                var users = await UserLoginService.ReadAllAsync();

                // Find the user with a matching username (case insensitive)
                var foundUser = users.FirstOrDefault(u =>
                    u.Username.Equals(UsernameModel.Username, StringComparison.OrdinalIgnoreCase));

                // If a matching user is found, generate a reset link
                if (foundUser != null)
                {
                    UsernameHandler.Username = foundUser.Username;

                    // Generate a dynamic reset link with UserId and UserUniqueId
                    resetLink = $"/reset-password/{foundUser.UserId}/{foundUser.UserUniqueId}";
                }
                else
                {
                    // Clear the reset link if no user is found
                    resetLink = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Log any errors encountered during the process
                Console.WriteLine($"Error finding user: {ex}");

                // Clear the reset link in case of an error
                resetLink = string.Empty;
            }
        }
        #endregion
    }
}
