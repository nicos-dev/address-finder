namespace FormValidator.interfaces.google.geocode.response
{

    /**
     * Object is used to deserialize response from Google-Geocode-API
     */
    class GeocodeResponseAddressComponent
    {
        /**
         * Example
         *      long_name: Germany
         *      short_name: DE
         *      types: ["country",...]
         */

        public string long_name;

        public string short_name;

        public string[] types;
    }
}
