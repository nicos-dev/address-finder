namespace FormValidator.interfaces.google.geocode.response
{
    /// <summary>
    /// Object used to deserialize Google Geocoding response data.
    /// </summary>
    public class GeocodeResponse
    {
        public GeocodeResponseResults[] results { get; set; }
    }
}
