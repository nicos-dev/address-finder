namespace FormValidator.interfaces.google.geocode.response
{

    /// <summary>
    /// Object used to deserialize Google Geocoding response data.
    /// </summary>
    public class GeocodeResponseAddressComponent
    {
        /**
         * Example
         *      long_name: Germany
         *      short_name: DE
         *      types: ["country",...]
         */

        public string long_name { get; set; }

        public string short_name { get; set; }

        public string[] types { get; set; }
    }
}
