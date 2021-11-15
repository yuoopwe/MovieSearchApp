using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.ThemovieDb
{
    class ThemovieDbModel
    {
        public int page { get; set; }
        public List<TheMovieDbListResultModel> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
}
