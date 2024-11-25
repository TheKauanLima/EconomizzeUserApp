using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

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

        /// <summary>
        /// Event callback to notify parent components when user details are updated.
        /// </summary>
        [Parameter]
        public EventCallback OnUserDetailsUpdated { get; set; }

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
                if (_userDetailModel == null) return;

                var userEntity = Mapper.Map<User>(_userDetailModel);

                if (UserService.CurrentEntity is null)
                {
                    await UserService.CreateAsync(userEntity);
                }
                else
                {
                    await UserService.UpdateAsync(userEntity);
                }

                // Notify parent about the update
                await OnUserDetailsUpdated.InvokeAsync();

                Console.WriteLine("User details saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user details: {ex.Message}");
            }

            StateHasChanged();
        }
        #endregion
    }
}
