using MovieSearchApp.Models.OMDb;
using MovieSearchApp.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services
{
    class OmdbService
    {
        private readonly IRestService _restService;
        private readonly string _apiKey;

        public OmdbService(IRestService restService, string apiKey)
        {
            _restService = restService;
            _apiKey = apiKey;
        }

        public async Task<MovieCollectionModel> GetMoviesAsync(string search, int pageCounter)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"?apikey={_apiKey}&s={search}&page={pageCounter}");

            return result.payload;
        }

        public async Task<MovieDetailsModel> GetMovieDetailsAsync(string id)
        {
            var result = await _restService.GetAsync<MovieDetailsModel>($"?apikey={_apiKey}&i={id}");

            return result.payload;
        }
    }
}
