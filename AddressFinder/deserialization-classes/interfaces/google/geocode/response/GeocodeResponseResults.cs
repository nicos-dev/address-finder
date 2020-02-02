namespace FormValidator.interfaces.google.geocode.response
{

    /// <summary>
    /// Object used to deserialize Google Geocoding response data.
    /// </summary>
    public class GeocodeResponseResults
    {
        // Address formatted by google // TODO use for displayed drop-down-tiles
        public string formatted_address { get; set; }

        // Unique ID of place (google)
        public string place_id { get; set; }

        // Type of place TODO: Further filtering
        public string[] types { get; set; }

        // Detailed information of place (street, number, city, postal-code, country)
        public GeocodeResponseAddressComponent[] address_components { get; set; }

    }
}
