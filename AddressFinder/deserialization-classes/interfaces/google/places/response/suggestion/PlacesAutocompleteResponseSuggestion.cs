﻿using System;

 namespace FormValidator.interfaces.google.places.response.suggestion
{
    /// <summary>
    /// Object used to deserialize Google Places response data.
    /// </summary>
    public class PlacesAutocompleteResponseSuggestion
    {
        public string name { get; set; }
        public string place_id { get; set; }
        // Type of place TODO: Do further filtering (Company, private address, etc)
        public string[] types { get; set; }

}
}
