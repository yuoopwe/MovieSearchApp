using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.OMDb
{
    public class MovieCollectionModel
    {
        public List<MovieModel> Search { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }

    }
}
