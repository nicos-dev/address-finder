using FormValidator.assembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FormValidator.interfaces.google.places.response;
using FormValidator.interfaces.search;
using FormValidator.services;
using Serilog;
using Serilog.Core;

namespace FormValidator
{

    public partial class MainForm : Form
    {
        #region -- member variables --

        private readonly Logger _log;
        private const string ApiKey = "AIzaSyDIF5SzlaOj13bbF65T03JOwt9UFvuhzY8";

        private const string PlacesApiUrl = "https://maps.googleapis.com/maps/api/place/textsearch/";
        private const string GeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/";

        private readonly DebounceDispatcher _debounceDispatcher;
        private readonly GooglePlacesApiService _googlePlacesApiService;
        private readonly GooglePlacesResponseAssembler _googlePlacesResponseAssembler;
        
        private List<SearchSuggestion> _searchSuggestions;
        private PlacesAutocompleteResponse _placesAutocompleteResponse;
        private string[] _currentDisplayedSuggestions = { };
        private string _selectedSearchItem = "";

        #endregion

        public MainForm()
        {
            InitializeComponent();

            _log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
            
            _googlePlacesApiService = new GooglePlacesApiService(PlacesApiUrl, ApiKey);
            _googlePlacesResponseAssembler = new GooglePlacesResponseAssembler(GeocodingApiUrl, ApiKey);
            _debounceDispatcher = new DebounceDispatcher(500);
            
            _searchSuggestions = new List<SearchSuggestion>();
        }

        #region -- triggerable methods --

        /// <summary>
        /// RUN SEARCH REQUEST
        /// After method is finished all old suggestions in drop-down
        /// will be removed and replaced by new suggestions based on
        /// search-field-input.
        /// 
        /// Note:  This process runs debounced, this prevents application
        ///        performance-issues due to many requests.
        /// </summary>
        /// <param name="sender">...</param>
        /// <param name="e">...</param>
        private void RunSearchRequest(object sender, EventArgs e)
        {
            // Filter if selected-item is 'placeholder' text
            if (comboBoxSearch.SelectedIndex == 0) return;
            
            // Start progress-bar
            progressBarSearch.Visible = true;
                
            /*
             * Debounce needed to prevent executing (updateSearchSuggestions() -triggered by action) every time text is changed.
             * So all inside of debounce runs with 500ms delay, if another call requested in these 500ms the 'Timer' is reset.
             */
            _debounceDispatcher.Debounce(() =>
            {
                try
                {
                    // Prevents from refresh suggestions when item in drop-down selected.
                    if (comboBoxSearch.Text.ToLower().Equals(_selectedSearchItem.ToLower())) return;
                    
                    // Execute
                    UpdateSearchSuggestions();
                    _log.Information($">UPDATED < Suggestions for [Text]:'{comboBoxSearch.Text}'");

                }
                catch (Exception ex)
                {
                    _log.Error($">FAILED  < [EX]:'{ex.GetType().Name}' Suggestions for [Text]:'{comboBoxSearch.Text}'");

                    // Error window
                    MessageBox.Show($@"Failed to update suggestions for '{ex.StackTrace}'.",
                        $@"Address Finder ({ex.StackTrace})",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    // Vanish progress-bar
                    progressBarSearch.Visible = false;
                }
            });

        }

        /// <summary>
        /// SET ADDRESS TEXT-FIELDS
        /// Triggered if user selects item in search-suggestions.
        /// </summary>
        /// <param name="sender">...</param>
        /// <param name="e">...</param>
        private void SetAddress(object sender, EventArgs e)
        {
            // Filter if selected-item is 'placeholder' text
            if (comboBoxSearch.SelectedIndex != 0)
            {
                // Get selected item from search-drop-down
                _selectedSearchItem = (string)comboBoxSearch.SelectedItem;

                _log.Information($">SELECT  < User selected '{_selectedSearchItem}' in [Object]: ComboBoxSearch");

                // Iterate through current set search-suggestions
                this._searchSuggestions.ForEach(item =>
                {
                    // Search matching item from selected item in drop-down in search-suggestions by name
                    if (!$"{item.Name}: {item.City}, {item.Country}".ToLower()
                        .Equals(_selectedSearchItem.ToLower())) return;

                    // Set text-box properties
                    textBoxCountry1.Text = item.Country ?? "";
                    textBoxCity.Text = item.City ?? "";
                    textBoxPostalCode.Text = item.PostalCode ?? "";
                    textBoxStreetName1.Text = item.StreetName ?? "";
                    textBoxHouseNumber.Text = item.HouseNumber ?? "";

                    _log.Information($">FINISHED< Address data set for '{_selectedSearchItem}'");
                });
            } else
            {
                textBoxCountry1.Text = "";
                textBoxCity.Text = "";
                textBoxPostalCode.Text = "";
                textBoxStreetName1.Text = "";
                textBoxHouseNumber.Text = "";
            }
            
        }
        
        #endregion

        #region -- private methods --
        
        /// <summary>
        /// UPDATE SEARCH-SUGGESTIONS
        /// 1. Remove old drop-down suggestions
        /// 2. Request new data & deserialize response
        /// 3. Set drop-down suggestions based on response
        /// </summary>
        private void UpdateSearchSuggestions()
        {
            // Remove all old suggestions from drop-down
            this.RemoveItemsFromSearchDropDown(_currentDisplayedSuggestions);

            // Execute text-search request against google-api and deserialize response.
            _placesAutocompleteResponse = _googlePlacesApiService.DoRequest(this.GetSearchText());

            // Convert response to search-suggestion and get further information (geocode)
            _searchSuggestions = this._googlePlacesResponseAssembler.ConvertToSearchSuggestion(_placesAutocompleteResponse.results);
            
            // Set new search-suggestions
            this.SetSearchBoxItems();
        }

        /// <summary>
        /// SET SEARCH-BOX DROP-DOWN ITEMS
        /// 
        /// 1. Create List based on SearchSuggestions
        ///     -> "{searchSuggestion.Name}: {searchSuggestion.City}, {searchSuggestion.Country}"
        ///     example: USU GmbH: City, Germany
        /// 2. Set drop-down items
        /// </summary>
        private void SetSearchBoxItems()
        {
            _currentDisplayedSuggestions = _searchSuggestions
                .Select(selector: searchSuggestion => 
                    $"{searchSuggestion.Name}: {searchSuggestion.City}, {searchSuggestion.Country}").ToArray();
            
            // Set items displayed in drop-down
            comboBoxSearch.Items.AddRange((object[]) _currentDisplayedSuggestions.Clone());
        }

        /// <summary>
        /// REMOVE OLD SUGGESTIONS IN DROP-DOWN
        /// 
        /// Combo-Box.Items (string[]) should not be cleared,
        /// this prevents ISSUE (Reset cursor in search-field).
        /// </summary>
        /// <param name="displayedSuggestions"></param>
        private void RemoveItemsFromSearchDropDown(IEnumerable<string> displayedSuggestions)
        {
            _log.Information(string.Format("Removed {0} suggestions from list",
                displayedSuggestions.Select(v => {
                    // Removing items prevents from reset cursor in search-field
                    // (If Clear() is used cursor jumps to start of the search-field)
                    comboBoxSearch.Items.Remove(v);
                    return v;
                }).Count()));
        }
        
        /// <summary>
        /// Get text inserted in search-box
        /// </summary>
        /// <returns>search-text</returns>
        private string GetSearchText()
        {
            return comboBoxSearch.Text;
        }
        
        #endregion
        
    }
}
