using System;
using FormValidator.interfaces.google.places.response.prediction;

namespace FormValidator.interfaces.google.places.response
{
    /**
     * Object used to deserialize response from Google-Places-API
     */
    class PlacesAutocompleteResponse
    {
        // Request-Status returned from google-api
        public String status { get; set; }
        
        // Search Results
        public PlacesAutocompleteResponsePrediction[] results { get; set; }
    }
}
