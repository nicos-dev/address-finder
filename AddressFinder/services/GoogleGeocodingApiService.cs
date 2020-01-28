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
        private Logger log;

        private static HttpClient client;

        /**
         * API keys are a simple encrypted string that can be used when calling certain APIs 
         * that don't need to access private user data. The API key 
         * is used to track API requests associated with project for quota and billing.
         * 
         *  -> Keep this key private!
         */
        private protected string apiKey;

        private string geocodeApiUrl;

        public GoogleGeocodingApiService(string geocodeApiUrl, string apiKey)
        {
            // Log
            log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client = new HttpClient();
            this.geocodeApiUrl = geocodeApiUrl;
            this.apiKey = apiKey;
        }
        
        /**
         * EXECUTE REQUEST
         */
        public Task<HttpResponseMessage> executePlaceIdHRGeocodeRequest(string placeId)
        {
            return client.GetAsync(string.Format("{0}json?place_id={1}&key={2}", geocodeApiUrl, placeId, apiKey));
        }

        /**
         * DESERIALIZE JSON TO GRA (Geocode-Response-Array)
         * 
         * @param json Json string to deserialize
         */
        public GeocodeResponse deserializeJsonToGRA(string json)
        {
            try
            {
                GeocodeResponse geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(json);
                log.Debug(">FINISHED< Convert Google-Geocode response");
                return geocodeResponse;
            } catch(Exception ex)
            {
                log.Error(">FAILED  < Convert Google-Geocode response");
                throw ex;
            }
            
        }

        /**
         * EXECUTE REQUEST AND WAIT FOR RESPONSE
         */
        private Task<HttpResponseMessage> executeGeocodingRequest(string place_id)
        {
            try
            {
                Task<HttpResponseMessage> taskRequest = this.executePlaceIdHRGeocodeRequest(place_id);
                this.waitForTaskToFinish(taskRequest);
                log.Information(String.Format(">FINISHED< Google-Geocode request for [Place-ID]:'{0}'", place_id));
                return taskRequest;
            } catch(Exception ex)
            {
                log.Error(String.Format(">FAILED  < Google-Geocode request for [Place-ID]:'{0}'", place_id));
                throw ex;
            }
        }

        /**
         * READ RESPONSE CONTENT
         */
        private string readResponseContent(Task<HttpResponseMessage> task)
        {
            try
            {
                Task<string> taskReadStream = task.Result.Content.ReadAsStringAsync();
                this.waitForTaskToFinish(taskReadStream);
                log.Debug(">FINISHED< Read content of Google-Geocode request");
                return taskReadStream.Result;
            } catch(Exception ex)
            {
                log.Error(">FAILED  < Read content of Google-Geocode request");
                throw ex;
            }
            
        }

        private void waitForTaskToFinish(Task task)
        {
            task.Wait();
        }

        /**
         * EXECUTE REQUEST AND DESERIALIZE RESPONSE CONTENT
         */
        public GeocodeResponse doRequest(string place_id)
        {
            try
            {
                GeocodeResponse geocodeResponse = this.deserializeJsonToGRA(
                    this.readResponseContent(
                            this.executeGeocodingRequest(place_id)
                        )
                    );
                log.Information(String.Format(">FINISHED< Get Google-Geocode information for [Place-ID]:'{0}'", place_id));
                return geocodeResponse;
            } catch(Exception ex)
            {
                log.Error(String.Format(">FAILED  < Get Google-Geocode information for [Place-ID]:'{0}'", place_id));
                throw ex;
            }

        }

    }
}
