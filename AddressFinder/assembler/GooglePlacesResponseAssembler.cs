using FormValidator.interfaces.google.geocode.response;
using FormValidator.services;
using System.Collections.Generic;
using System.Linq;
using FormValidator.interfaces.google.places.response.suggestion;
using FormValidator.interfaces.search;

namespace FormValidator.assembler
{
    /// <summary>
    /// Used to enhance Places data using Google Geocoding API and
    /// create suggestions based on both data.
    /// </summary>
    public class GooglePlacesResponseAssembler
    {

        #region -- member variables --

        private readonly GoogleGeocodingApiService _googleGeocodingApiService;

        #endregion
        
        #region -- constructor --

        /// <summary>
        /// Constructor of GooglePlacesResponseAssembler
        /// </summary>
        /// <param name="geocodingApiUrl">Url where Google Geocoding API is available</param>
        /// <param name="apiKey">API-Key generated and Geocoding API is activated</param>
        public GooglePlacesResponseAssembler(string geocodingApiUrl, string apiKey)
        {
            _googleGeocodingApiService = new GoogleGeocodingApiService(
                geocodeApiUrl: geocodingApiUrl, 
                apiKey: apiKey);
        }

        #endregion

        #region -- public methods --

        /// <summary>
        /// Converts response data from Google Places API to SearchSuggestion,
        /// which is enhanced by Google Geocoding data.
        /// </summary>
        /// <param name="suggestions">List of suggestions (Google Places text search response)</param>
        /// <returns>List of generated Objects based on Places & Geocoding data</returns>
        public List<SearchSuggestion> ConvertToSearchSuggestion(PlacesAutocompleteResponseSuggestion[] suggestions)
        {

            return suggestions
                .Where(v => v != null)
                .Select((suggestion) =>
                {
                    // Init new SearchSuggestion for each item in list
                    var searchSuggestion = new SearchSuggestion
                    {
                        Name = suggestion.name, PlaceId = suggestion.place_id, Types = suggestion.types
                    };

                    // Set information from google-places-api

                    // Request Google-Geocode-API (get geocode information by Place-ID)
                    var geocodeResponse = _googleGeocodingApiService.DoRequest(placeId: suggestion.place_id);

                    // Check if response contains information about Place-ID
                    if (geocodeResponse.results.Any(geocode => geocode.place_id == suggestion.place_id))
                    {
                        // Get first matching result
                        var geocodeFound = geocodeResponse.results.First(geocode => geocode.place_id == suggestion.place_id);

                        /*
                         * Set properties
                         * 
                         * Information found?
                         *  -> true (set property)
                         *  -> false (set property to empty string)
                         */
                        searchSuggestion.FormattedAddress = geocodeFound.formatted_address;

                        searchSuggestion.StreetName = geocodeFound.address_components.Any(address => address.types.Contains("route"))
                            ? geocodeFound.address_components.First(address => address.types.Contains("route")).long_name
                            : "";

                        searchSuggestion.HouseNumber = geocodeFound.address_components.Any(address => address.types.Contains("street_number"))
                            ? geocodeFound.address_components.First(address => address.types.Contains("street_number")).long_name
                            : "";

                        searchSuggestion.City = geocodeFound.address_components.Any(address => address.types.Contains("locality"))
                            ? geocodeFound.address_components.First(address => address.types.Contains("locality")).long_name
                            : "";

                        searchSuggestion.PostalCode = geocodeFound.address_components.Any(address => address.types.Contains("postal_code"))
                            ? geocodeFound.address_components.First(address => address.types.Contains("postal_code")).long_name
                            : "";

                        searchSuggestion.Country = geocodeFound.address_components.Any(address => address.types.Contains("country"))
                            ? geocodeFound.address_components.First(address => address.types.Contains("country")).long_name
                            : "";
                        

                    } else
                    {
                        // If no further information about place exists properties set to empty string
                        searchSuggestion.FormattedAddress = "";
                        searchSuggestion.StreetName = "";
                        searchSuggestion.HouseNumber = "";
                        searchSuggestion.City = "";
                        searchSuggestion.PostalCode = "";
                        searchSuggestion.Country = "";
                        
                    }
                    return searchSuggestion;
                })
                .ToList();
        }

        #endregion

    }
}
