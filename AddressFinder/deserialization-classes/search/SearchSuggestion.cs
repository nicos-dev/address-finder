namespace FormValidator.interfaces.search
{

    /**
     * Used to save data from Google Places & Geocode in one object.
     */

    class SearchSuggestion
    {
        public string place_id;
        public string name;
        public string[] types;
        public string streetName;
        public string houseNumber;
        public string city;
        public string postalCode;
        public string country;
        public string formatted_address;
    }
}
