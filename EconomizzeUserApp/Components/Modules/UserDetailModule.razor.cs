using Economizze.Library;
using EconomizzeUserApp.Model;

namespace EconomizzeUserApp.Components.Modules
{
    public partial class UserDetailModule
    {
        private UserDetailModel _userDetailModel = new UserDetailModel(); // Model for the form

        protected override void OnInitialized()
        {
            _userDetailModel.UserId = UserLoginService.CurrentEntity!.UserId;
        }

        private async Task HandleSave()
        {
            if (UserService.CurrentEntity is null)
            {
                var user = Mapper.Map<User>(_userDetailModel);
                await UserService.CreateAsync(user);
            }
            else
            {
                var user = Mapper.Map<User>(_userDetailModel);
                await UserService.UpdateAsync(user);
            }
        }
    }
}