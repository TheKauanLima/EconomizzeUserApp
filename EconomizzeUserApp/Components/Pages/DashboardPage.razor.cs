namespace EconomizzeUserApp.Components.Pages
{
    public partial class DashboardPage
    {
        private string activeTab = "Quotes";

        private void SelectTab(string tabName)
        {
            activeTab = tabName;
        }
    }
}