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
        private readonly string _baseUrl;

        public OmdbService(IRestService restService, string apiKey, string baseUrl)
        {
            _restService = restService;
            _apiKey = apiKey;
            _baseUrl = baseUrl;
        }

        public async Task<MovieCollectionModel> GetAllAsync(string search, int pageCounter)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={_apiKey}&s={search}&page={pageCounter}");

            return result.payload;
        }

        public async Task<MovieCollectionModel> GetSeriesAsync(string search, int pageCounter)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={_apiKey}&s={search}&page={pageCounter}&type=series");

            return result.payload;
        }
        public async Task<MovieCollectionModel> GetMoviesAsync(string search, int pageCounter)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={_apiKey}&s={search}&page={pageCounter}&type=movie");

            return result.payload;
        }
        public async Task<MovieCollectionModel> GetGamesAsync(string search, int pageCounter)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={_apiKey}&s={search}&page={pageCounter}&type=game");

            return result.payload;
        }

        public async Task<MovieDetailsModel> GetMovieDetailsAsync(string id)
        {
            var result = await _restService.GetAsync<MovieDetailsModel>($"{_baseUrl}?apikey={_apiKey}&i={id}");

            return result.payload;
        }

        public async Task<MovieDetailsModel> GetTitleAsync(string title, string year)
        {
            var result = await _restService.GetAsync<MovieDetailsModel>($"{_baseUrl}?apikey={_apiKey}&t={title}&y={year}");

            return result.payload;
        }
    }
}
