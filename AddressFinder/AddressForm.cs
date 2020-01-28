using FormValidator.assembler;
using FormValidator.interfaces.google.places.response;
using FormValidator.interfaces.search;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FormValidator
{
    public struct Request { };

    public partial class MainForm : Form
    {
        // LOGGING
        private Logger log;

        // DBEOUNCE CLASS
        private DebounceDispatcher debounceTimer;

        // SERVICES
        private GooglePlacesApiService googlePlacesApiService;

        // ASSEMBLER
        private GooglePlacesResponseAssembler googlePlacesResponseAssembler;

        // SEARCH PREDICTION
        private List<SearchSuggestion> searchPredictions; 

        // GOOGLE-PLACES RESPONSE
        private PlacesAutocompleteResponse placesAutocompleteResponse;

        // CURRENT DISPLAYED ITEMS IN DROP-DOWN
        private string[] currentDisplayedPredictions = { };

        // SELECTED DROP-DOWN ITEM
        private string selectedSearchItem = "";

        public MainForm()
        {
            InitializeComponent();

            // Style settings
            this.Size = new System.Drawing.Size(400, 240);
            comboBoxSearch.Size = new System.Drawing.Size(362, 21);
            comboBoxSearch.Location = new System.Drawing.Point(13, 6);
            comboBoxSearch.Items.Insert(0, "Search...");
            comboBoxSearch.SelectedIndex = 0;

            /**
             * INIT Objects
             */

            // Log
            log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            // Services
            googlePlacesApiService = new GooglePlacesApiService();

            // Assembler
            googlePlacesResponseAssembler = new GooglePlacesResponseAssembler();

            // Debounce
            debounceTimer = new DebounceDispatcher();

            // Search predictions
            searchPredictions = new List<SearchSuggestion>();
        }

        private void runSearchRequest(object sender, EventArgs e)
        {
            /**
             * Debounce needed to prevent executing (updateSearchSuggestions() -triggered by action) every time text is changed.
             * So the Method is only once in 500 ms executable. This prevents the application from 
             * hanging due to outdated requests to the API, this happens because there will be 
             * more and more requests when you type text.
             */
            if (comboBoxSearch.SelectedIndex != 0)
            {
                debounceTimer.Debounce(500, (p) =>
                {
                    try
                    {
                        // Prevents from refresh predictions when item in drop-down selected.
                        if (!comboBoxSearch.Text.ToLower().Equals(selectedSearchItem.ToLower()))
                        {
                            // Start progress-bar
                            progressBarSearch.Visible = true;
                            progressBarSearch.Value = 0;

                            updateSearchSuggestions();
                            log.Information(String.Format(">UPDATED < Suggestions for [Text]:'{0}'", comboBoxSearch.Text));
                        }
                    
                    } catch(Exception ex)
                    {
                        log.Error(String.Format(">FAILED  < [EX]:'{0}' Suggestions for [Text]:'{1}'", ex.GetType().Name, comboBoxSearch.Text));
                        MessageBox.Show(String.Format("Failed to update suggestions for '{0}'.", comboBoxSearch.Text), String.Format("Adress Finder ({0})", ex.GetType().FullName),
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    } finally
                    {
                        // Reset progress-bar
                        progressBarSearch.Value = 0;
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
        private void setAddress(object sender, EventArgs e)
        {
            if (comboBoxSearch.SelectedIndex != 0)
            {
                // Get selected item from search-drop-down
                selectedSearchItem = (string)comboBoxSearch.SelectedItem;

                log.Information(String.Format(">SELECT  < User selected '{0}' in [Object]: ComboBoxSearch", selectedSearchItem));

                // Iterate through current set search-predictions
                this.searchPredictions.ForEach(item =>
                {
                    // Search matching item from search-predictions by name set in drop-down
                    if (String.Format("{0}: {1}, {2}", item.name, item.city, item.country).ToLower().Equals(selectedSearchItem.ToLower()))
                    {

                        // Set text-box properties
                        if (item.country != null)
                        {
                            textBoxCountry1.Text = item.country;
                        }
                        if (item.city != null)
                        {
                            textBoxCity.Text = item.city;
                        }
                        if (item.postalCode != null)
                        {
                            textBoxPostalCode.Text = item.postalCode;
                        }
                        if (item.streetName != null)
                        {
                            textBoxStreetName1.Text = item.streetName;
                        }
                        if (item.houseNumber != null)
                        {
                            textBoxHouseNumber.Text = item.houseNumber;
                        }
                        log.Information(String.Format(">FINISHED< Address data set for '{0}'", selectedSearchItem));
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
        private void updateSearchSuggestions()
        {
            // Remove all old suggestions from drop-down
            this.removeItemsFromSearchDropDown(currentDisplayedPredictions);

            progressBarSearch.Value = 40;

            // Execute textsearch request against google-api and deserialize response.
            placesAutocompleteResponse = googlePlacesApiService.doRequest(this.getSearchText());

            // Convert response to search-suggestion and get further information (geocode)
            searchPredictions = this.googlePlacesResponseAssembler.convertToSearchSuggestion(placesAutocompleteResponse.results);

            progressBarSearch.Value = 90;

            // Set new search-suggestions
            this.setSearchBoxItems(searchPredictionList: searchPredictions);

            progressBarSearch.Value = 100;
        }

        /**
         * SET SEARCH-BOX DROP-DOWN ITEMS
         * 
         * Sets usefull displayed name in drop-down by geocode information
         */
        private void setSearchBoxItems(List<SearchSuggestion> searchPredictionList)
        {
            List<string> predictionsList = new List<string>();
            foreach (var searchPrediction in searchPredictions)
            {
                predictionsList.Add(String.Format("{0}: {1}, {2}", searchPrediction.name, searchPrediction.city, searchPrediction.country));
            }

            // Set items displayed in drop-down
            comboBoxSearch.Items.AddRange(predictionsList.ToArray());

            // Saves items to be able to check if data changes
            currentDisplayedPredictions = predictionsList.ToArray();
        }

        /**
         * REMOVE ITEMS SET IN DROP-DOWN
         */
        private void removeItemsFromSearchDropDown(string[] displayedPredictions)
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
        private string getSearchText()
        {
            return comboBoxSearch.Text;
        }

        private void TextBoxCity_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
