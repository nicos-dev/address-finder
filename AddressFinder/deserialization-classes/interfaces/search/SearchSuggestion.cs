namespace FormValidator.interfaces.search
{

    /**
     * Used to save data from Google Places & Geocode in one object.
     */

    class SearchSuggestion
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string[] Types { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string FormattedAddress { get; set; }
    }
}
