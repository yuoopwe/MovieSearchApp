using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services.Rest
{
    public class RestService : IRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _host;

        public RestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        public async Task<(ResultStatus status, TResponse payload, string rawResponse)> GetAsync<TResponse>(string path)
        {

           
            HttpResponseMessage response = await _httpClient.GetAsync(Sanitise(path));

            if (response.IsSuccessStatusCode == true)
            {
                string rawData = await response.Content.ReadAsStringAsync();

                // Turn our JSON string into a csharp object (or object-graph)
                var result = JsonConvert.DeserializeObject<TResponse>(rawData);

                // Return a response of type TResult.
                return (ResultStatus.Success, result, rawData);
            }
            else
            {
                return (ResultStatus.Other, default(TResponse), null);
            }
        }

        public async Task<(ResultStatus status, TResponse payload, string rawResponse)> PostAsync<TRequest, TResponse>(TRequest request, string path)
        {
            

            string json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(Sanitise(path), data);

            if (response.IsSuccessStatusCode == true)
            {
                string rawData = await response.Content.ReadAsStringAsync();

                // Turn our JSON string into a csharp object (or object-graph)
                TResponse result = JsonConvert.DeserializeObject<TResponse>(rawData);

                // Return a response of type TResult.
                return (ResultStatus.Success, result, rawData);

            }
            else
            {
                return (ResultStatus.Other, default(TResponse), null);
            }
        }

        private string Sanitise(string uri)
        {
            return uri.Replace('\\', '/');
        }
    }
}