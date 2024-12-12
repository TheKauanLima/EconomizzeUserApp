namespace EconomizzeUserApp.Components.Pages
{
    public partial class DashboardPage
    {
        private string activeTab = "Detalhes Pessoais";

        private void SelectTab(string tabName)
        {
            activeTab = tabName;
        }

        private async Task RefreshUserData()
        {
            if (UserService.CurrentEntity?.UserId != null)
            {
                // Re-fetch user data using ReadByIdAsync
                var updatedUser = await UserService.ReadByIdAsync(UserService.CurrentEntity.UserId);
                if (updatedUser is not null)
                {
                    UserService.CurrentEntity = updatedUser;
                }
                StateHasChanged();
            }
        }
    }
}