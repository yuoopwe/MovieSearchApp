using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.UserAccount
{
    class JournalDetailsModel
    {
        public string MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieRating { get; set; }
        public string MovieComments { get; set; }
        public string MovieRuntime { get; set; }
    }
}
