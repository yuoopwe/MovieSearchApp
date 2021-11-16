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
        private readonly IRestService _restService;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public TastediveService(IRestService restService, string apiKey, string baseUrl)
        {
            _restService = restService;
            _apiKey = apiKey;
            _baseUrl = baseUrl;
        }

        public async Task<RecommendationModel> GetRecommendationsMovie(string search)
        {
            var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={_apiKey}&q=movie:{search}&info=1");

            return result.payload;
        }

        public async Task<RecommendationModel> GetRecommendations(string search)
        {
            var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={_apiKey}&q={search}&info=1");

            return result.payload;
        }

        public async Task<RecommendationModel> GetRecommendationsByType(string search, string type)
       { 
           var result = await _restService.GetAsync<RecommendationModel>($"{_baseUrl}similar?k={_apiKey}&q={type}{search}&info=1");

           return result.payload;
       }
    }
}
