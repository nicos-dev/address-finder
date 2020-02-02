using System;
using System.Collections.Generic;
using FormValidator.interfaces.google.places.response.suggestion;

namespace FormValidator.interfaces.google.places.response
{

    /// <summary>
    /// Object used to deserialize Google Places response data.
    /// </summary>
    public class PlacesAutocompleteResponse
    {
        public string status { get; set; }
        public PlacesAutocompleteResponseSuggestion[] results { get; set; }
    }
}
