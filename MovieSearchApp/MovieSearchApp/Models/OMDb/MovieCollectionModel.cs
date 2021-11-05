using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.OMDb
{
    public class MovieCollectionModel
    {
        public MovieCollectionModel(List<MovieModel> search, string totalresults, string response)
        {
            Search = search;
            totalResults = totalresults;
            Response = response;
        }
        public List<MovieModel> Search { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }

    }
}
