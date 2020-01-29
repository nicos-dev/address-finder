using FormValidator.assembler;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FormValidator.interfaces.google.places.response;
using FormValidator.interfaces.search;
using FormValidator.services;
using Serilog;
using Serilog.Core;

namespace FormValidator
{
    public struct Request { };

    public partial class MainForm : Form
    {
        // LOGGING
        private readonly Logger _log;
        
        // API-KEY
        private static readonly string _apiKey = "<your-api-key";

        // DEBOUNCE CLASS
        private readonly DebounceDispatcher _debounceDispatcher;

        // SERVICES
        private readonly GooglePlacesApiService _googlePlacesApiService;

        // ASSEMBLER
        private readonly GooglePlacesResponseAssembler _googlePlacesResponseAssembler;

        // SEARCH PREDICTION
        private List<SearchSuggestion> _searchPredictions; 

        // GOOGLE-PLACES RESPONSE
        private PlacesAutocompleteResponse _placesAutocompleteResponse;

        // CURRENT DISPLAYED ITEMS IN DROP-DOWN
        private string[] _currentDisplayedPredictions = { };

        // SELECTED DROP-DOWN ITEM
        private string _selectedSearchItem = "";

        public MainForm()
        {
            InitializeComponent();

            /*
             * INIT Objects
             */

            // Log
            _log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            // Services
            _googlePlacesApiService = new GooglePlacesApiService(_apiKey);

            // Assembler
            _googlePlacesResponseAssembler = new GooglePlacesResponseAssembler(_apiKey);

            // Debounce
            _debounceDispatcher = new DebounceDispatcher(500);

            // Search predictions
            _searchPredictions = new List<SearchSuggestion>();
        }

        private void RunSearchRequest(object sender, EventArgs e)
        {
            /*
             * Debounce needed to prevent executing (updateSearchSuggestions() -triggered by action) every time text is changed.
             * So the Method is only once in 500 ms executable. This prevents the application from 
             * hanging due to outdated requests to the API, this happens because there will be 
             * more and more requests when you type text.
             */
            if (comboBoxSearch.SelectedIndex != 0)
            {
                // Start progress-bar
                progressBarSearch.Visible = true;
                _debounceDispatcher.Debounce(() =>
                {
                    try
                    {
                        // Prevents from refresh predictions when item in drop-down selected.
                        if (!comboBoxSearch.Text.ToLower().Equals(_selectedSearchItem.ToLower()))
                        {
                            UpdateSearchSuggestions();
                            _log.Information($">UPDATED < Suggestions for [Text]:'{comboBoxSearch.Text}'");
                        }

                    } catch(Exception ex)
                    {
                        _log.Error(
                            $">FAILED  < [EX]:'{ex.GetType().Name}' Suggestions for [Text]:'{comboBoxSearch.Text}'");
                        MessageBox.Show($@"Failed to update suggestions for '{comboBoxSearch.Text}'.",
                            $@"Address Finder ({ex.GetType().FullName})",
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    } finally
                    {
                        // Reset progress-bar
                        progressBarSearch.Visible = false;
                    }
                
                });
            }
            
        }

        /**
         * SET ADDRESS TEXT-FIELDS BASED ON GEOCODE
         * 
         * -> Executed if user select prediction in search drop-down
         */
        private void SetAddress(object sender, EventArgs e)
        {
            if (comboBoxSearch.SelectedIndex != 0)
            {
                // Get selected item from search-drop-down
                _selectedSearchItem = (string)comboBoxSearch.SelectedItem;

                _log.Information($">SELECT  < User selected '{_selectedSearchItem}' in [Object]: ComboBoxSearch");

                // Iterate through current set search-predictions
                this._searchPredictions.ForEach(item =>
                {
                    // Search matching item from search-predictions by name set in drop-down
                    if ($"{item.Name}: {item.City}, {item.Country}".ToLower().Equals(_selectedSearchItem.ToLower()))
                    {

                        // Set text-box properties
                        if (item.Country != null)
                        {
                            textBoxCountry1.Text = item.Country;
                        }
                        if (item.City != null)
                        {
                            textBoxCity.Text = item.City;
                        }
                        if (item.PostalCode != null)
                        {
                            textBoxPostalCode.Text = item.PostalCode;
                        }
                        if (item.StreetName != null)
                        {
                            textBoxStreetName1.Text = item.StreetName;
                        }
                        if (item.HouseNumber != null)
                        {
                            textBoxHouseNumber.Text = item.HouseNumber;
                        }
                        _log.Information($">FINISHED< Address data set for '{_selectedSearchItem}'");
                    }
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

        /**
         * UPDATE SEARCH-SUGGESTIONS
         * 
         */
        private void UpdateSearchSuggestions()
        {
            // Remove all old suggestions from drop-down
            this.RemoveItemsFromSearchDropDown(_currentDisplayedPredictions);

            // Execute text-search request against google-api and deserialize response.
            _placesAutocompleteResponse = _googlePlacesApiService.DoRequest(this.GetSearchText());

            // Convert response to search-suggestion and get further information (geocode)
            _searchPredictions = this._googlePlacesResponseAssembler.ConvertToSearchSuggestion(_placesAutocompleteResponse.results);

            // Set new search-suggestions
            this.SetSearchBoxItems();
        }

        /**
         * SET SEARCH-BOX DROP-DOWN ITEMS
         * 
         * Sets useful displayed name in drop-down by geocode information
         */
        private void SetSearchBoxItems()
        {
            List<string> predictionsList = new List<string>();
            foreach (var searchPrediction in _searchPredictions)
            {
                predictionsList.Add($"{searchPrediction.Name}: {searchPrediction.City}, {searchPrediction.Country}");
            }

            // Set items displayed in drop-down
            comboBoxSearch.Items.AddRange(predictionsList.ToArray());

            // Saves items to be able to check if data changes
            _currentDisplayedPredictions = predictionsList.ToArray();
        }

        /**
         * REMOVE ITEMS SET IN DROP-DOWN
         */
        private void RemoveItemsFromSearchDropDown(string[] displayedPredictions)
        {
            foreach (var item in displayedPredictions)
            {
                // Removing items prevents from reset cursor in search-field
                // (If Clear() is used cursor jumps to start of the search-field)
                comboBoxSearch.Items.Remove(item);
            }
        }

        /**
         * GET THE SEARCH-TEXT ENTERED IN THE SEARCH-FIELD
         */
        private string GetSearchText()
        {
            return comboBoxSearch.Text;
        }

        private void TextBoxCity_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
