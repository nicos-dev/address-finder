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

    class GooglePlacesApiService
    {

        // LOGGING
        private readonly Logger _log;

        private static HttpClient _client;
        private readonly String _apiKey;

        public GooglePlacesApiService(string apiKey)
        {
            // Log
            _log = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client = new HttpClient();
            this._apiKey = apiKey;
        }

        private Task<HttpResponseMessage> ExecuteSearchRequest(string searchText)
        {
            return _client.GetAsync(
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={searchText}&key={_apiKey}");
        }


        private PlacesAutocompleteResponse DeserializeJsonToPar(string json)
        {
            try
            {
                PlacesAutocompleteResponse placesAutocompleteResponse = JsonConvert.DeserializeObject<PlacesAutocompleteResponse>(json);
                _log.Debug(">FINISHED< Convert Google-Places response");
                return placesAutocompleteResponse;
            } catch (Exception)
            {
                _log.Debug(">FAILED  < Convert Google-Places response");
                throw;
            }
            
        }

        /**
         * EXECUTE REQUEST AND WAIT FOR RESPONSE
         */
        private Task<HttpResponseMessage> ExecutePlacesRequest(string searchText)
        {
            try
            {
                Task<HttpResponseMessage> taskRequest = this.ExecuteSearchRequest(searchText);
                this.waitForTaskToFinish(taskRequest);
                _log.Information($">FINISHED< Google-Places request for [Text]:'{searchText}'");
                return taskRequest;
            } catch(Exception)
            {
                _log.Error($">FAILED  < Google-Places request for [Text]:'{searchText}'");
                throw;
            }
        }

        private string ReadResponseContent(Task<HttpResponseMessage> task)
        {
            try
            {
                Task<string> taskReadStream = task.Result.Content.ReadAsStringAsync();
                this.waitForTaskToFinish(taskReadStream);
                _log.Debug(">FINISHED< Read content of Google-Places request");
                return taskReadStream.Result;
            } catch(Exception)
            {
                _log.Error(">FAILED  < Read content of Google-Geocode request");
                throw;
            }
            
        }

        private void waitForTaskToFinish(Task task)
        {
            task.Wait();
        }

        public PlacesAutocompleteResponse DoRequest(string searchText)
        {
            try
            {
                PlacesAutocompleteResponse placesAutocompleteResponse = this.DeserializeJsonToPar(
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

    }
}
