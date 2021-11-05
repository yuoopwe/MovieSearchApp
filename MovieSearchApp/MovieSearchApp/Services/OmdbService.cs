﻿using MovieSearchApp.Models.OMDb;
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

        public async Task<MovieCollectionModel> GetMoviesAsync(string search)
        {
            var result = await _restService.GetAsync<MovieCollectionModel>($"?apikey={_apiKey}&s={search}");

            return result.payload;
        }
    }
}
