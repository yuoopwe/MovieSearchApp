using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.OMDb
{
    public class RatingsModel
    {
        public RatingsModel(string source, string value)
        {
            Source = source;
            Value = value;
        }

        public string Source { get; set; }
        public string Value { get; set; }
    }
}
