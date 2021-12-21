using MovieSearchApp.Models.Tastedive;
using MovieSearchApp.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services
{
    class TastediveService
    {
        private readonly KeyVaultService _keyVaultService;
        private readonly IRestService _restService;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public TastediveService(IRestService restService, string baseUrl, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _restService = restService; 
            _baseUrl = baseUrl;
        }

        public async Task<RecommendationModel> GetRecommendationsMovie(string search)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={secrets.TastediveApiKey}&q=movie:{search}&info=1");

            return result.payload;
        }

        public async Task<RecommendationModel> GetRecommendations(string search)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={secrets.TastediveApiKey}&q={search}&info=1");

            return result.payload;
        }

        public async Task<RecommendationModel> GetRecommendationsByType(string search, string type)
       {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={secrets.TastediveApiKey}&q={type}{search}&info=1");

           return result.payload;
       }
    }
}
