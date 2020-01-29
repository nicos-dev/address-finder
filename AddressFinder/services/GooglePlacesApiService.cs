using System;
using System.Threading.Tasks;
using System.Net.Http;
using FormValidator.interfaces.google.places.response;
using Newtonsoft.Json;
using System.Net;
using Serilog.Core;
using Serilog;

namespace FormValidator
{

    class GooglePlacesApiService
    {

        // LOGGING
        private Logger log;

        private static HttpClient client;
        private protected String apiKey;

        public GooglePlacesApiService(string apiKey)
        {
            // Log
            log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client = new HttpClient();
            this.apiKey = apiKey;
        }

        private Task<HttpResponseMessage> executeSearchRequest(string searchText)
        {
            return client.GetAsync(string.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&key={1}", searchText, apiKey));
        }


        private PlacesAutocompleteResponse deserializeJsonToPAR(string json)
        {
            try
            {
                PlacesAutocompleteResponse placesAutocompleteResponse = JsonConvert.DeserializeObject<PlacesAutocompleteResponse>(json);
                log.Debug(">FINISHED< Convert Google-Places response");
                return placesAutocompleteResponse;
            } catch (Exception ex)
            {
                log.Debug(">FAILED  < Convert Google-Places response");
                throw ex;
            }
            
        }

        /**
         * EXECUTE REQUEST AND WAIT FOR RESPONSE
         */
        private Task<HttpResponseMessage> executePlacesRequest(string searchText)
        {
            try
            {
                Task<HttpResponseMessage> taskRequest = this.executeSearchRequest(searchText);
                this.waitForTaskToFinish(taskRequest);
                log.Information(String.Format(">FINISHED< Google-Places request for [Text]:'{0}'", searchText));
                return taskRequest;
            } catch(Exception ex)
            {
                log.Error(String.Format(">FAILED  < Google-Places request for [Text]:'{0}'", searchText));
                throw ex;
            }
        }

        private string readResponseContent(Task<HttpResponseMessage> task)
        {
            try
            {
                Task<string> taskReadStream = task.Result.Content.ReadAsStringAsync();
                this.waitForTaskToFinish(taskReadStream);
                log.Debug(">FINISHED< Read content of Google-Places request");
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

        public PlacesAutocompleteResponse doRequest(string searchText)
        {
            try
            {
                PlacesAutocompleteResponse placesAutocompleteResponse = this.deserializeJsonToPAR(
                    this.readResponseContent(
                            this.executePlacesRequest(searchText)
                        )
                    );
                log.Information(String.Format(">FINISHED< Get Google-Places information for [Text]:'{0}'", searchText));
                return placesAutocompleteResponse;
            } catch(Exception ex)
            {
                log.Error(String.Format(">FAILED  < Get Google-Places information for [Text]:'{0}'", searchText));
                throw ex;
            }
            
        }

    }
}
