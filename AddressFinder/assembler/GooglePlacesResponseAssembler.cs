using FormValidator.interfaces.google.geocode.response;
using FormValidator.services;
using System.Collections.Generic;
using System.Linq;
using FormValidator.interfaces.google.places.response.suggestion;
using FormValidator.interfaces.search;

namespace FormValidator.assembler
{
    class GooglePlacesResponseAssembler
    {
        // Services
        readonly GoogleGeocodingApiService _googleGeocodingApiService;

        public GooglePlacesResponseAssembler(string apiKey)
        {
            // Init service & passing params
            _googleGeocodingApiService = new GoogleGeocodingApiService(
                geocodeApiUrl: "https://maps.googleapis.com/maps/api/geocode/", 
                apiKey: apiKey);
        }

        /**
         * CONVERTS GOOGLE-PLACES RESPONSE TO SEARCH-SUGGESTION INCLUDING MORE DETAILS
         * 
         */
        public List<SearchSuggestion> ConvertToSearchSuggestion(PlacesAutocompleteResponsePrediction[] predictions)
        {

            List<SearchSuggestion> searchPredictionList = new List<SearchSuggestion>();

            // Iterate through deserialized response from google-places-api
            foreach (var prediction in predictions)
            {
                // Init new SearchSuggestion for each item in list
                SearchSuggestion searchPrediction = new SearchSuggestion
                {
                    Name = prediction.name, PlaceId = prediction.place_id, Types = prediction.types
                };

                // Set information from google-places-api

                // Request Google-Geocode-API (get geocode information by Place-ID)
                GeocodeResponse geocodeResponse = _googleGeocodingApiService.DoRequest(placeId: prediction.place_id);

                // Check if response contains information about Place-ID
                if (geocodeResponse.results.Any(geocode => geocode.place_id == prediction.place_id))
                {
                    // Get first matching result
                    GeocodeResponseResults geocodeFound = geocodeResponse.results.First(geocode => geocode.place_id == prediction.place_id);

                    /*
                     * Set properties
                     * 
                     * Information found?
                     *  -> true (set property)
                     *  -> false (set property to empty string)
                     */
                    searchPrediction.FormattedAddress = geocodeFound.formatted_address;

                    searchPrediction.StreetName = geocodeFound.address_components.Any(address => address.types.Contains("route"))
                        ? geocodeFound.address_components.First(address => address.types.Contains("route")).long_name
                        : "";

                    searchPrediction.HouseNumber = geocodeFound.address_components.Any(address => address.types.Contains("street_number"))
                        ? geocodeFound.address_components.First(address => address.types.Contains("street_number")).long_name
                        : "";

                    searchPrediction.City = geocodeFound.address_components.Any(address => address.types.Contains("locality"))
                        ? geocodeFound.address_components.First(address => address.types.Contains("locality")).long_name
                        : "";

                    searchPrediction.PostalCode = geocodeFound.address_components.Any(address => address.types.Contains("postal_code"))
                        ? geocodeFound.address_components.First(address => address.types.Contains("postal_code")).long_name
                        : "";

                    searchPrediction.Country = geocodeFound.address_components.Any(address => address.types.Contains("country"))
                        ? geocodeFound.address_components.First(address => address.types.Contains("country")).long_name
                        : "";
                    

                } else
                {
                    // If no further information about place exists properties set to empty string
                    searchPrediction.FormattedAddress = "";
                    searchPrediction.StreetName = "";
                    searchPrediction.HouseNumber = "";
                    searchPrediction.City = "";
                    searchPrediction.PostalCode = "";
                    searchPrediction.Country = "";
                    
                }

                searchPredictionList.Add(searchPrediction);
                
            }
            return searchPredictionList;
        }


       

    }
}
