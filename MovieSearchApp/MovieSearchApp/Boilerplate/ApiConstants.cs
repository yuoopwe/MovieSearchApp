using System.Configuration;

namespace MovieSearchApp.Boilerplate
{
    public class ApiConstants
    {
        public const string OmdbBaseApiUrl = "http://www.omdbapi.com/";
        public static string OmdbApiKey = ConfigurationManager.ConnectionStrings["OmdbApiKey"].ConnectionString;
        public const string TastediveBaseApiUrl = "https://tastedive.com/api/";
        public static string TastediveApiKey = ConfigurationManager.ConnectionStrings["TastediveApiKey"].ConnectionString;
        public const string TheMovieDbBaseApiUrl = "https://api.themoviedb.org/3/";
        public static string TheMovieDbApiKey = ConfigurationManager.ConnectionStrings["TheMovieDbApiKey"].ConnectionString;


    }
}