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


        public async Task<ThemovieDbModel> DiscoverMovies(string search)
        {
            var result = await _restService.GetAsync<ThemovieDbModel>($"{_baseUrl}/discover/movie?api_key={_apiKey}&primary_release_year=2010&sort_by=vote_average.desc");

            return result.payload;
        }
    }
}

