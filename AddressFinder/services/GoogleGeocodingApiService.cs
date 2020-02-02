using FormValidator.interfaces.google.geocode.response;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FormValidator.services
{
    /// <summary>
    /// Service used to get geocoding information about a Google Place (Place-ID)
    /// </summary>
    public class GoogleGeocodingApiService
    {

        #region -- member variables --

        private readonly Logger _log;
        
        private static HttpClient _client;

        /**
         * API keys are a simple encrypted string that can be used when calling certain APIs 
         * that don't need to access private user data. The API key 
         * is used to track API requests associated with project for quota and billing.
         * 
         *  -> Keep this key private!
         */
        private readonly string _apiKey;

        private readonly string _geocodeApiUrl;

        #endregion
        
        # region -- constructor --
        /// <summary>
        /// Constructor of GoogleGeocodingApiService
        /// </summary>
        /// <param name="geocodeApiUrl">Url where Google Geocoding API is available</param>
        /// <param name="apiKey">API-Key generated and Geocoding API is activated</param>
        public GoogleGeocodingApiService(string geocodeApiUrl, string apiKey)
        {
            // Log
            _log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client = new HttpClient();
            this._geocodeApiUrl = geocodeApiUrl;
            this._apiKey = apiKey;
        }
        #endregion

        #region -- private methods --

        /// <summary>
        /// Execute request with Http-Client
        /// </summary>
        /// <param name="placeId">Unique Place-ID given by Google</param>
        /// <returns>Unfinished task of request</returns>
        private Task<HttpResponseMessage> ExecutePlaceIdHrGeocodeRequest(string placeId)
        {
            return _client.GetAsync($"{_geocodeApiUrl}json?place_id={placeId}&key={_apiKey}");
        }

        /// <summary>
        /// Deserialize JSON Response to Object 
        /// </summary>
        /// <param name="json">JSON-String (Response-Body)</param>
        /// <returns>Created object based on JSON data</returns>
        private GeocodeResponse DeserializeJsonToGr(string json)
        {
            try
            {
                var geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(json);
                _log.Debug(">FINISHED< Convert Google-Geocode response");
                return geocodeResponse;
            } catch(Exception)
            {
                _log.Error(">FAILED  < Convert Google-Geocode response");
                throw;
            }
            
        }

        /// <summary>
        /// Execute request and wait until task finished
        /// </summary>
        /// <param name="placeId">Unique Place-ID given by Google</param>
        /// <returns>Finished Task of Request</returns>
        private Task<HttpResponseMessage> ExecuteGeocodingRequest(string placeId)
        {
            try
            {
                var taskRequest = this.ExecutePlaceIdHrGeocodeRequest(placeId);
                WaitForTaskToFinish(taskRequest);
                _log.Information($">FINISHED< Google-Geocode request for [Place-ID]:'{placeId}'");
                return taskRequest;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Google-Geocode request for [Place-ID]:'{placeId}'");
                throw;
            }
        }

        /// <summary>
        /// Read Response Body JSON-String
        /// </summary>
        /// <param name="task">Finished task of request</param>
        /// <returns>JSON-String</returns>
        private string ReadResponseContent(Task<HttpResponseMessage> task)
        {
            try
            {
                var taskReadStream = task.Result.Content.ReadAsStringAsync();
                WaitForTaskToFinish(taskReadStream);
                _log.Debug(">FINISHED< Read content of Google-Geocode request");
                return taskReadStream.Result;
            } catch(Exception)
            {
                _log.Error(">FAILED  < Read content of Google-Geocode request");
                throw;
            }
            
        }

        /// <summary>
        /// Wait until task finished
        /// </summary>
        /// <param name="task">Task to wait for finish</param>
        private static void WaitForTaskToFinish(Task task)
        {
            task.Wait();
        }
        
        #endregion

        #region -- public methods --

        /// <summary>
        /// Do request
        ///  > Execute Request
        ///  > Read response content
        ///  > Deserialize object
        /// </summary>
        /// <param name="placeId">Unique Place-ID given by Google</param>
        /// <returns>Object based on response data</returns>
        public GeocodeResponse DoRequest(string placeId)
        {
            try
            {
                var geocodeResponse = this.DeserializeJsonToGr(
                    this.ReadResponseContent(
                            this.ExecuteGeocodingRequest(placeId)
                        )
                    );
                _log.Information($">FINISHED< Get Google-Geocode information for [Place-ID]:'{placeId}'");
                return geocodeResponse;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Get Google-Geocode information for [Place-ID]:'{placeId}'");
                throw;
            }

        }
        #endregion
        
    }
}
