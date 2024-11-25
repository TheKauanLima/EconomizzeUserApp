using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Component for searching and managing addresses based on ZIP codes.
    /// Allows users to search for street details, add new addresses, edit, and delete existing addresses.
    /// </summary>
    public partial class SearchZipCodeModule
    {
        [Parameter]
        public bool ShowActions { get; set; } = true;

        /// <summary>
        /// Holds the details of the street retrieved based on the ZIP code.
        /// </summary>
        private StreetDetailViewModel _streetDetailViewModel = new();

        /// <summary>
        /// Stores address-specific details for the user.
        /// </summary>
        private UserAddressModel _userAddressModel = new();

        /// <summary>
        /// Model to capture the ZIP code entered by the user.
        /// </summary>
        private ZipCodeModel _zipCodeModel = new();

        /// <summary>
        /// Flag indicating whether a search attempt was made.
        /// </summary>
        private bool _searchAttempted = false;

        /// <summary>
        /// Holds the current user's address details for display.
        /// </summary>
        private UserAddress? _currentUserAddress;

        #region LIFECYCLE METHODS

        /// <summary>
        /// Initializes the component and loads the current address.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            StreetService.SetListValues();
            StreetDetailViewService.SetListValues();
            UserAddressService.SetListValues();

            // Load the current address on initialization
            await LoadCurrentAddress();
        }
        #endregion

        #region SEARCH METHODS

        /// <summary>
        /// Handles searching for street details based on the ZIP code entered by the user.
        /// </summary>
        private async Task HandleSearch()
        {
            _searchAttempted = true;

            try
            {
                // Fetch street details based on ZIP code
                var streetDetailModel = await StreetDetailViewService.ReadStreetDetailByZipCodeAsync(_zipCodeModel.ZipCode);
                _streetDetailViewModel = streetDetailModel != null
                    ? Mapper.Map<StreetDetailViewModel>(streetDetailModel)
                    : new StreetDetailViewModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during ZIP code search: {ex.Message}");
                _streetDetailViewModel = new StreetDetailViewModel();
            }
        }
        #endregion

        #region ADDRESS MANAGEMENT METHODS

        /// <summary>
        /// Handles adding a new address based on the current street details.
        /// </summary>
        private async Task HandleAdd()
        {
            if (_streetDetailViewModel is null || _userAddressModel is null) return;

            try
            {
                // Retrieve UserId from the settings
                var userId = SettingsService.appSettings.UserId;

                // Map and add UserAddress entity
                var newUserAddress = new UserAddress
                {
                    UserId = userId,
                    StreetId = _streetDetailViewModel.StreetId,
                    Complement = _userAddressModel.Complement,
                    Longitude = _userAddressModel.Longitude,
                    Latitude = _userAddressModel.Latitude,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.Now
                };

                // Add new street details and user address
                var streetDetailView = Mapper.Map<StreetDetailView>(_streetDetailViewModel);
                await StreetDetailViewService.CreateAsync(streetDetailView);
                await UserAddressService.CreateAsync(newUserAddress);

                // Reload the current address after adding
                await LoadCurrentAddress();

                // Clear form fields after adding
                ResetForm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding address: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the current user's address based on their UserId.
        /// </summary>
        private async Task LoadCurrentAddress()
        {
            try
            {
                var userId = SettingsService.appSettings.UserId;
                _currentUserAddress = (await UserAddressService.ReadAllAsync())
                    .LastOrDefault(ua => ua.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading current address: {ex.Message}");
                _currentUserAddress = null;
            }
        }

        /// <summary>
        /// Allows editing the current address by populating the form with existing data.
        /// </summary>
        private void EditCurrentAddress()
        {
            if (_currentUserAddress == null) return;

            // Map the current address to the form fields for editing
            _userAddressModel = Mapper.Map<UserAddressModel>(_currentUserAddress);
        }

        /// <summary>
        /// Deletes the current address of the user.
        /// </summary>
        private async Task DeleteCurrentAddress()
        {
            if (_currentUserAddress == null) return;

            try
            {
                await UserAddressService.DeleteAsync(_currentUserAddress.UserAddressId);
                _currentUserAddress = null;

                // Clear form fields after deletion
                ResetForm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting address: {ex.Message}");
            }
        }
        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Resets the form fields and state flags.
        /// </summary>
        private void ResetForm()
        {
            _streetDetailViewModel = new StreetDetailViewModel();
            _userAddressModel = new UserAddressModel();
            _zipCodeModel = new ZipCodeModel();
            _searchAttempted = false;
        }
        #endregion
    }
}
