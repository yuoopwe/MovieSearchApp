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
        private readonly IRestService _restService;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public ThemoviedbService(IRestService restService, string apiKey, string baseUrl)
        {
            _restService = restService;
            _apiKey = apiKey;
            _baseUrl = baseUrl;
        }


        public async Task<ThemovieDbModel> DiscoverMovies(string search,int pageCounter)
        {
            var result = await _restService.GetAsync<ThemovieDbModel>($"{_baseUrl}discover/movie?api_key={_apiKey}&primary_release_year={search}&sort_by=popularity.desc&region=GB&page={pageCounter}");

            return result.payload;
        }

        public async Task<ThemovieDbModel> DiscoverMoviesGenre(string search, int pageCounter, int genreId)
        {
            var result = await _restService.GetAsync<ThemovieDbModel>($"{_baseUrl}discover/movie?api_key={_apiKey}&primary_release_year={search}&sort_by=popularity.desc&region=GB&page={pageCounter}&with_genres&with_genres={genreId}");

            return result.payload;
        }


    }
}


