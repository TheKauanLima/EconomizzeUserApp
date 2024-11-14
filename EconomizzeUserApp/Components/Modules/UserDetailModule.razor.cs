using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component for managing user details. 
    /// Allows users to update or save their profile information.
    /// </summary>
    public partial class UserDetailModule
    {
        /// <summary>
        /// Model used to capture user details in the form.
        /// </summary>
        private UserDetailModel _userDetailModel = new();

        #region LIFECYCLE METHODS

        /// <summary>
        /// Initializes the component by setting the UserId from the currently logged-in user.
        /// </summary>
        protected override void OnInitialized()
        {
            // Set the UserId of the current user
            _userDetailModel.UserId = UserLoginService.CurrentEntity?.UserId ?? 0;
        }
        #endregion

        #region USER SAVE HANDLER

        /// <summary>
        /// Handles saving or updating user details.
        /// If the user does not exist, a new entry is created; otherwise, the existing entry is updated.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleSave()
        {
            try
            {
                // Ensure that the UserDetailModel is not null before proceeding
                if (_userDetailModel == null)
                {
                    Console.WriteLine("UserDetailModel is null.");
                    return;
                }

                // Map the UserDetailModel to a User entity
                var userEntity = Mapper.Map<User>(_userDetailModel);

                if (UserService.CurrentEntity is null)
                {
                    // Create a new user if none exists
                    await UserService.CreateAsync(userEntity);
                }
                else
                {
                    // Update the existing user
                    await UserService.UpdateAsync(userEntity);
                }

                // Provide feedback upon successful save
                Console.WriteLine("User details saved successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error saving user details: {ex.Message}");
            }
        }
        #endregion
    }
}
