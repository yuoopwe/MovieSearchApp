using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.UserAccount
{
    class JournalDetailsModel
    {

        public string MovieID { get; set; }
        public string MovieTitle { get; set; }
      
        private string _movieRating;

        public string MovieRating
        {
            get => _movieRating;
            set
            {
                if (value.Contains("/ 10"))
                {
                    _movieRating = value;
                }
                else
                {
                    _movieRating = value + " / 10";
                }

            }
        }
        public string MovieComments { get; set; }
        public string MovieRuntime { get; set; }
    }
}
