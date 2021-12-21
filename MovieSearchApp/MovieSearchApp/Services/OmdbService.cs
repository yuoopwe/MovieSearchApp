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
        private readonly KeyVaultService _keyVaultService;
        private readonly IRestService _restService;
        private readonly string _baseUrl;

        public OmdbService(IRestService restService, string baseUrl, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _restService = restService;
            _baseUrl = baseUrl;
        }


        public async Task<MovieCollectionModel> GetAllAsync(string search, int pageCounter)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&s={search}&page={pageCounter}");

            return result.payload;
        }


        public async Task<MovieCollectionModel> GetSeriesAsync(string search, int pageCounter)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&s={search}&page={pageCounter}&type=series");

            return result.payload;
        }
        public async Task<MovieCollectionModel> GetMoviesAsync(string search, int pageCounter)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&s={search}&page={pageCounter}&type=movie");

            return result.payload;
        }
        public async Task<MovieCollectionModel> GetGamesAsync(string search, int pageCounter)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieCollectionModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&s={search}&page={pageCounter}&type=game");

            return result.payload;
        }

        public async Task<MovieDetailsModel> GetMovieDetailsWithIdAsync(string id)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieDetailsModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&i={id}");

            return result.payload;
        }

        public async Task<MovieDetailsModel> GetMovieDetailsWithTitleAndYearAsync(string title, string year)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieDetailsModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&t={title}&y={year}");

            return result.payload;
        }
        public async Task<MovieDetailsModel> GetMovieDetailsWithTitleAsync(string title)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<MovieDetailsModel>($"{_baseUrl}?apikey={secrets.OmdbApiKey}&t={title}");

            return result.payload;
        }
    }
}
