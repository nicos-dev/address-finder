﻿using System;

 namespace FormValidator.interfaces.google.places.response.suggestion
{
    /**
     * Object is used to deserialize response from Google-Places-API
     */
    class PlacesAutocompleteResponsePrediction
    {
        // Basicly what you searched for
        public String name { get; set; }

        // Unique Place-ID
        public String place_id { get; set; }

        // Type of place TODO: Do further filtering (Company, private address, etc)
        public string[] types { get; set; }

}
}
