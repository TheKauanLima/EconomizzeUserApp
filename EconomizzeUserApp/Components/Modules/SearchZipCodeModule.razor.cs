using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class SearchZipCodeModule
    {
        private StreetDetailViewModel _streetDetailViewModel = new StreetDetailViewModel(); // For street details
        private UserAddressModel _userAddressModel = new UserAddressModel(); // For address-specific details
        private ZipCodeModel _zipCodeModel = new ZipCodeModel();
        private bool _searchAttempted = false;
        private UserAddress? _currentUserAddress; // To hold the current address for display

        protected override async Task OnInitializedAsync()
        {
            StreetService.SetListValues();
            StreetDetailViewService.SetListValues();
            UserAddressService.SetListValues();
            await LoadCurrentAddress();
        }

        private async Task HandleSearch()
        {
            _searchAttempted = true;
            var streetDetailModel = await StreetDetailViewService.ReadStreetDetailByZipCodeAsync(_zipCodeModel.ZipCode);
            _streetDetailViewModel = streetDetailModel != null ? Mapper.Map<StreetDetailViewModel>(streetDetailModel) : new StreetDetailViewModel();
        }

        private async Task HandleAdd()
        {
            if (_streetDetailViewModel is not null && _userAddressModel is not null)
            {
                // Retrieve UserId from SettingsService
                var userId = SettingsService.appSettings.UserId;

                // Map and add UserAddress entity with the retrieved StoreId
                var newUserAddress = new UserAddress
                {
                    UserId = userId,
                    StreetId = _streetDetailViewModel.StreetId, // Ensure StreetId is set or default to 0
                    Complement = _userAddressModel.Complement,
                    Longitude = _userAddressModel.Longitude,
                    Latitude = _userAddressModel.Latitude,
                    CreatedBy = 1,   // Example value; replace with actual user ID
                    CreatedOn = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                };

                var streetDetailView = Mapper.Map<StreetDetailView>(_streetDetailViewModel);
                await StreetDetailViewService.CreateAsync(streetDetailView);

                await UserAddressService.CreateAsync(newUserAddress);

                // Reload the current address after adding
                await LoadCurrentAddress();

                // Clear form after adding
                _streetDetailViewModel = new StreetDetailViewModel();
                _userAddressModel = new UserAddressModel();
                _zipCodeModel = new ZipCodeModel(); // Reset the search field as well
                _searchAttempted = false;
            }
        }

        private async Task LoadCurrentAddress()
        {
            // Retrieve the current address for the store using UserId from SettingsService
            var userId = SettingsService.appSettings.UserId;
            _currentUserAddress = (await UserAddressService.ReadAllAsync())
                .LastOrDefault(ua => ua.UserId == userId); // Assuming the most recent address
        }

        private void EditCurrentAddress()
        {
            if (_currentUserAddress != null)
            {
                // Map the current address to the form fields for editing
                _userAddressModel = Mapper.Map<UserAddressModel>(_currentUserAddress);
                _userAddressModel.UserId = _currentUserAddress.UserId;
                _userAddressModel.StreetId = _currentUserAddress.StreetId;
                _userAddressModel.Complement = _currentUserAddress.Complement;
                _userAddressModel.Longitude = _currentUserAddress.Longitude;
                _userAddressModel.Latitude = _currentUserAddress.Latitude;
                // _userAddressModel.CreatedBy = _currentStoreAddress.CreatedBy;
                // _userAddressModel.CreatedOn = _currentStoreAddress.CreatedOn;
                // _userAddressModel.ModifiedBy = _currentStoreAddress.ModifiedBy;
                // _userAddressModel.ModifiedOn = DateTime.Now;
            }
        }

        private async Task DeleteCurrentAddress()
        {
            if (_currentUserAddress != null)
            {
                await UserAddressService.DeleteAsync(_currentUserAddress.UserAddressId);
                _currentUserAddress = null; // Clear the current address after deletion
            }
        }
    }
}