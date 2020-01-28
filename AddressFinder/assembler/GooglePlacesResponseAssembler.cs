using FormValidator.interfaces.google.geocode.response;
using FormValidator.interfaces.google.places.response.prediction;
using FormValidator.interfaces.search;
using FormValidator.services;
using System.Collections.Generic;
using System.Linq;

namespace FormValidator.assembler
{
    class GooglePlacesResponseAssembler
    {
        // Services
        GoogleGeocodingApiService googleGeocodingApiService;

        public GooglePlacesResponseAssembler()
        {
            // Init service & passing params
            googleGeocodingApiService = new GoogleGeocodingApiService(
                geocodeApiUrl: "https://maps.googleapis.com/maps/api/geocode/", 
                apiKey: "<your-api-key>");
        }

        /**
         * CONVERTS GOOGLE-PLACES RESPONSE TO SEARCH-SUGGESTION INCLUDING MORE DETAILS
         * 
         */
        public List<SearchSuggestion> convertToSearchSuggestion(PlacesAutocompleteResponsePrediction[] predictions)
        {

            List<SearchSuggestion> searchPredictionList = new List<SearchSuggestion>();

            // Iterate through deserialized response from google-places-api
            foreach (var prediction in predictions)
            {
                // Init new SearchSuggestion for each item in list
                SearchSuggestion searchPrediction = new SearchSuggestion();

                // Set information from google-places-api
                searchPrediction.name = prediction.name;
                searchPrediction.place_id = prediction.place_id;
                searchPrediction.types = prediction.types;

                // Request Google-Geocode-API (get geocode informations by Place-ID)
                GeocodeResponse geocodeResponse = googleGeocodingApiService.doRequest(place_id: prediction.place_id);

                // Check if response contains information about Place-ID
                if (geocodeResponse.results.Where(geocode => geocode.place_id == prediction.place_id).Any())
                {
                    // Get first matching result
                    GeocodeResponseResults geocodeFound = geocodeResponse.results.Where(geocode => geocode.place_id == prediction.place_id).First();

                    /**
                     * Set properties
                     * 
                     * Information found?
                     *  -> true (set property)
                     *  -> false (set property to empty string)
                     */
                    searchPrediction.formatted_address = geocodeFound.formatted_address;

                    searchPrediction.streetName = geocodeFound.address_components.Where(address => address.types.Contains("route")).Any()
                                            ? geocodeFound.address_components.Where(address => address.types.Contains("route")).First().long_name
                                            : "";

                    searchPrediction.houseNumber = geocodeFound.address_components.Where(address => address.types.Contains("street_number")).Any()
                                            ? geocodeFound.address_components.Where(address => address.types.Contains("street_number")).First().long_name
                                            : "";

                    searchPrediction.city = geocodeFound.address_components.Where(address => address.types.Contains("locality")).Any()
                                            ? geocodeFound.address_components.Where(address => address.types.Contains("locality")).First().long_name
                                            : "";

                    searchPrediction.postalCode = geocodeFound.address_components.Where(address => address.types.Contains("postal_code")).Any()
                                            ? geocodeFound.address_components.Where(address => address.types.Contains("postal_code")).First().long_name
                                            : "";

                    searchPrediction.country = geocodeFound.address_components.Where(address => address.types.Contains("country")).Any()
                        ? geocodeFound.address_components.Where(address => address.types.Contains("country")).First().long_name
                        : "";
                    

                } else
                {
                    // If no further information about place exists propertys set to empty string
                    searchPrediction.formatted_address = "";
                    searchPrediction.streetName = "";
                    searchPrediction.houseNumber = "";
                    searchPrediction.city = "";
                    searchPrediction.postalCode = "";
                    searchPrediction.country = "";
                    
                }

                searchPredictionList.Add(searchPrediction);
                
            }
            return searchPredictionList;
        }


       

    }
}
