using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.OMDb
{
    public class MovieModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
