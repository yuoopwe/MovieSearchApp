using MovieSearchApp.Models.ThemovieDb;
using MovieSearchApp.Services.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchApp.Services
{
    class ThemoviedbService
    {
        private readonly KeyVaultService _keyVaultService;
        private readonly IRestService _restService;
        private readonly string _baseUrl;

        public ThemoviedbService(IRestService restService, string baseUrl, KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            _restService = restService;
            _baseUrl = baseUrl;
        }


        public async Task<TheMovieDbDetailsModel> DiscoverMoviesID(string id)
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<TheMovieDbDetailsModel>($"{_baseUrl}movie/{id}movie?api_key={secrets.TheMovieDbApiKey}");

            return result.payload;
        }

        public async Task<ThemovieDbModel> DiscoverMovies(string year, int pageCounter, string genreId, string language, string rating )
        {
            var secrets = await _keyVaultService.GetKeysAsync();

            var result = await _restService.GetAsync<ThemovieDbModel>($"{_baseUrl}discover/movie?api_key={secrets.TheMovieDbApiKey}&primary_release_year={year}&sort_by=popularity.desc&page={pageCounter}&with_genres={genreId}&with_original_language={language}&certification_country=US&certification={rating}");

            return result.payload;
        }


    }
}


