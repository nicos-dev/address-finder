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
    class GoogleGeocodingApiService
    {

        // LOGGING
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
        
        /**
         * EXECUTE REQUEST
         */
        private Task<HttpResponseMessage> ExecutePlaceIdHrGeocodeRequest(string placeId)
        {
            return _client.GetAsync($"{_geocodeApiUrl}json?place_id={placeId}&key={_apiKey}");
        }

        /**
         * DESERIALIZE JSON TO GRA (Geocode-Response-Array)
         * 
         * @param json Json string to deserialize
         */
        private GeocodeResponse DeserializeJsonToGra(string json)
        {
            try
            {
                GeocodeResponse geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(json);
                _log.Debug(">FINISHED< Convert Google-Geocode response");
                return geocodeResponse;
            } catch(Exception)
            {
                _log.Error(">FAILED  < Convert Google-Geocode response");
                throw;
            }
            
        }

        /**
         * EXECUTE REQUEST AND WAIT FOR RESPONSE
         */
        private Task<HttpResponseMessage> ExecuteGeocodingRequest(string placeId)
        {
            try
            {
                Task<HttpResponseMessage> taskRequest = this.ExecutePlaceIdHrGeocodeRequest(placeId);
                WaitForTaskToFinish(taskRequest);
                _log.Information($">FINISHED< Google-Geocode request for [Place-ID]:'{placeId}'");
                return taskRequest;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Google-Geocode request for [Place-ID]:'{placeId}'");
                throw;
            }
        }

        /**
         * READ RESPONSE CONTENT
         */
        private string ReadResponseContent(Task<HttpResponseMessage> task)
        {
            try
            {
                Task<string> taskReadStream = task.Result.Content.ReadAsStringAsync();
                WaitForTaskToFinish(taskReadStream);
                _log.Debug(">FINISHED< Read content of Google-Geocode request");
                return taskReadStream.Result;
            } catch(Exception)
            {
                _log.Error(">FAILED  < Read content of Google-Geocode request");
                throw;
            }
            
        }

        private static void WaitForTaskToFinish(Task task)
        {
            task.Wait();
        }

        /**
         * EXECUTE REQUEST AND DESERIALIZE RESPONSE CONTENT
         */
        public GeocodeResponse DoRequest(string placeId)
        {
            try
            {
                GeocodeResponse geocodeResponse = this.DeserializeJsonToGra(
                    this.ReadResponseContent(
                            this.ExecuteGeocodingRequest(placeId)
                        )
                    );
                _log.Information($">FINISHED< Get Google-Geocode information for [Place-ID]:'{placeId}'");
                return geocodeResponse;
            } catch(Exception ex)
            {
                _log.Error($">FAILED  < Get Google-Geocode information for [Place-ID]:'{placeId}'");
                throw;
            }

        }

    }
}
