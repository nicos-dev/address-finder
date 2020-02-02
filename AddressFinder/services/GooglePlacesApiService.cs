using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FormValidator.interfaces.google.places.response;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace FormValidator.services
{
    public class GooglePlacesApiService
    {
        
        #region -- member variables --

        private readonly Logger _log;
        private static HttpClient _client;
        private readonly string _placesApiUrl;
        
        /**
         * API keys are a simple encrypted string that can be used when calling certain APIs 
         * that don't need to access private user data. The API key 
         * is used to track API requests associated with project for quota and billing.
         * 
         *  -> Keep this key private!
         */
        private readonly string _apiKey;

        #endregion

        #region -- constructor --

        /// <summary>
        /// Constructor of GooglePlacesApiService
        /// </summary>
        /// <param name="placesApiUrl">Url where Google Places API is available</param>
        /// <param name="apiKey">API-Key generated and Places API is activated</param>
        public GooglePlacesApiService(string placesApiUrl, string apiKey)
                {
                    _log = new LoggerConfiguration()
                        .WriteTo.Console()
                        .MinimumLevel.Debug()
                        .CreateLogger();
        
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    _client = new HttpClient();
                    this._placesApiUrl = placesApiUrl;
                    this._apiKey = apiKey;
                }
        
        #endregion

        #region -- private methods --

        /// <summary>
        /// Execute request with Http-Client
        /// </summary>
        /// <param name="searchText">Text to search for</param>
        /// <returns>Unfinished task of request</returns>
        private Task<HttpResponseMessage> ExecuteSearchRequest(string searchText)
        {
            return _client.GetAsync(
                $"{_placesApiUrl}json?query={searchText}&key={_apiKey}");
        }


        /// <summary>
        /// Deserialize JSON Response to Object 
        /// </summary>
        /// <param name="json">JSON-String (Response-Body)</param>
        /// <returns>Created object based on JSON data</returns>
        private PlacesAutocompleteResponse DeserializeJsonToPar(string json)
        {
            try
            {
                var placesAutocompleteResponse = JsonConvert.DeserializeObject<PlacesAutocompleteResponse>(json);
                _log.Debug(">FINISHED< Convert Google-Places response");
                return placesAutocompleteResponse;
            } catch (Exception)
            {
                _log.Debug(">FAILED  < Convert Google-Places response");
                throw;
            }
            
        }

        /// <summary>
        /// Execute request and wait until task finished
        /// </summary>
        /// <param name="searchText">Text to search for</param>
        /// <returns>Finished task of request</returns>
        private Task<HttpResponseMessage> ExecutePlacesRequest(string searchText)
        {
            try
            {
                var taskRequest = this.ExecuteSearchRequest(searchText);
                WaitForTaskToFinish(taskRequest);
                _log.Information($">FINISHED< Google-Places request for [Text]:'{searchText}'");
                return taskRequest;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Google-Places request for [Text]:'{searchText}'");
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
                _log.Debug(">FINISHED< Read content of Google-Places request");
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
        /// <param name="searchText">Text to search for</param>
        /// <returns>Object based on response data</returns>
        public PlacesAutocompleteResponse DoRequest(string searchText)
        {
            try
            {
                var placesAutocompleteResponse = this.DeserializeJsonToPar(
                    this.ReadResponseContent(
                            this.ExecutePlacesRequest(searchText)
                        )
                    );
                _log.Information($">FINISHED< Get Google-Places information for [Text]:'{searchText}'");
                return placesAutocompleteResponse;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Get Google-Places information for [Text]:'{searchText}'");
                throw;
            }
            
        }

        #endregion

    }
}
