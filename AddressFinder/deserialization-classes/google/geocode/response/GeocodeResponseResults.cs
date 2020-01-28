namespace FormValidator.interfaces.google.geocode.response
{

    /**
     * Object is used to deserialize response from Google-Geocode-API
     */

    class GeocodeResponseResults
    {
        // Address formatted by google // TODO use for displayed drop-down-tiles
        public string formatted_address;

        // Unique ID of place (google)
        public string place_id;

        // Type of place TODO: Further filtering
        public string[] types;

        // Detailed information of place (street, number, city, postal-code, country)
        public GeocodeResponseAddressComponent[] address_components;

    }
}
