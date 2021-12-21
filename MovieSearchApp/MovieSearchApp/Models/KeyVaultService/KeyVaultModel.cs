using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.KeyVaultService
{
    class KeyVaultModel
    {
        public KeyVaultModel(string connectionString, string omdbApiKey, string tastediveApiKey, string theMovieDbApiKey)
        {
            ConnectionString = connectionString;
            OmdbApiKey = omdbApiKey;
            TastediveApiKey = tastediveApiKey;
            TheMovieDbApiKey = theMovieDbApiKey;
        }

        public string ConnectionString { get; }
        public string OmdbApiKey { get; }
        public string TastediveApiKey { get; }
        public string TheMovieDbApiKey { get; }
    }
}
