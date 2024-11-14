using Economizze.Library;
using EconomizzeUserApp.Model;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modules
{
    /// <summary>
    /// Module to manage quotes, including creation, viewing, and deletion.
    /// </summary>
    public partial class QuoteModule
    {
        /// <summary>
        /// List of quotes displayed in the UI.
        /// </summary>
        private List<QuoteModel> _quotes = new();

        /// <summary>
        /// Model for creating a new quote.
        /// </summary>
        private QuoteModel _newQuoteModel = new();

        /// <summary>
        /// UI state management flags.
        /// </summary>
        private bool isDrawerOpen { get; set; } = false;
        private bool isSearchZipCodeVisible { get; set; } = false;
        private bool useCurrentAddress { get; set; } = false;

        #region INITIALIZATION

        /// <summary>
        /// Initializes the component by loading necessary data.
        /// Sets default values and fetches the list of quotes.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Set list values if not already initialized
            if (_quotes.Count == 0 && QuoteService.CurrentEntity is null)
            {
                QuoteService.SetListValues();
            }

            StreetDetailViewService.SetListValues();
            await LoadQuotes();
        }
        #endregion

        #region LOAD DATA

        /// <summary>
        /// Loads all quotes from the service and maps them to the model.
        /// </summary>
        private async Task LoadQuotes()
        {
            try
            {
                var quotes = await QuoteService.ReadAllAsync();
                _quotes = Mapper.Map<List<QuoteModel>>(quotes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading quotes: {ex}");
            }
        }
        #endregion

        #region CREATE NEW QUOTE

        /// <summary>
        /// Determines if a new quote can be created based on the current address or selected entity.
        /// </summary>
        private bool CanCreateQuote => useCurrentAddress || StreetDetailViewService.CurrentEntity != null;

        /// <summary>
        /// Creates a new quote based on the selected or current address.
        /// Navigates to the prescription page upon success.
        /// </summary>
        private async Task CreateQuote()
        {
            // Ensure a valid NeighborhoodId is set
            if (StreetDetailViewService.CurrentEntity?.NeighborhoodId == 0) return;

            InitializeNewQuote();

            try
            {
                // Map the model to the entity and create the quote
                var newQuoteEntity = Mapper.Map<Quote>(_newQuoteModel);
                var createdQuote = await QuoteService.CreateAsync(newQuoteEntity);

                // Map the created entity back to the model and add it to the list
                var quoteModel = Mapper.Map<QuoteModel>(createdQuote);
                _quotes.Add(quoteModel);

                // Navigate to the prescriptions page
                NavigationManager.NavigateTo("/prescricao");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating quote: {ex}");
            }
        }

        /// <summary>
        /// Initializes a new quote with default values.
        /// </summary>
        private void InitializeNewQuote()
        {
            var userId = SettingsService.appSettings.UserId;
            _newQuoteModel = new QuoteModel
            {
                UserId = userId,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                IsExpired = false,
                NeighborhoodId = useCurrentAddress && StreetDetailViewService.CurrentEntity != null
                    ? StreetDetailViewService.CurrentEntity.NeighborhoodId
                    : (StreetDetailViewService.CurrentEntity?.NeighborhoodId ?? 0)
            };
        }
        #endregion

        #region NAVIGATION & DELETION

        /// <summary>
        /// Navigates to the prescriptions page for a specific quote.
        /// </summary>
        /// <param name="quoteId">The ID of the quote to view.</param>
        private void ViewQuote(int quoteId)
        {
            NavigationManager.NavigateTo($"/prescriptions/{quoteId}");
        }

        /// <summary>
        /// Deletes a quote by its ID and refreshes the quote list.
        /// </summary>
        /// <param name="quoteId">The ID of the quote to delete.</param>
        private async Task DeleteQuote(int quoteId)
        {
            try
            {
                await QuoteService.DeleteAsync(quoteId);
                await LoadQuotes(); // Refresh the list after deletion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting quote: {ex}");
            }
        }
        #endregion

        #region UI TOGGLES

        /// <summary>
        /// Toggles the state of the drawer menu.
        /// </summary>
        private void ToggleDrawer()
        {
            isDrawerOpen = !isDrawerOpen;

            // Reset drawer state when closed
            if (!isDrawerOpen)
            {
                ResetDrawerState();
            }
        }

        /// <summary>
        /// Toggles the visibility of the search by zip code module.
        /// </summary>
        private void ToggleSearchZipCodeModule()
        {
            isSearchZipCodeVisible = !isSearchZipCodeVisible;
        }

        /// <summary>
        /// Resets the drawer-related UI state.
        /// </summary>
        private void ResetDrawerState()
        {
            isSearchZipCodeVisible = false;
            useCurrentAddress = false;
        }
        #endregion
    }
}
