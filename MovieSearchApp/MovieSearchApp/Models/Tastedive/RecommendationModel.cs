using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Models.Tastedive
{
    class RecommendationModel
    {
        public RecommendationModel(Similar similar)
        {
            Similar = similar;
        }

        public Similar Similar { get; set; }
    }
    public class Similar
    {
        public Similar(List<Info> info, List<Result> results)
        {
            Info = info;
            Results = results;
        }

        public List<Info> Info { get; set; }
        public List<Result> Results { get; set; }
    }
    public class Info
    {
        public Info(string name, string type, string wTeaser, string wUrl, string yUrl, string yID)
        {
            Name = name;
            Type = type;
            this.wTeaser = wTeaser;
            this.wUrl = wUrl;
            this.yUrl = yUrl;
            this.yID = yID;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string wTeaser { get; set; }
        public string wUrl { get; set; }
        public string yUrl { get; set; }
        public string yID { get; set; }
    }

    public class Result
    {
        public Result(string name, string type, string wTeaser, string wUrl, string yUrl, string yID)
        {
            Name = name;
            Type = type;
            this.wTeaser = wTeaser;
            this.wUrl = wUrl;
            this.yUrl = yUrl;
            this.yID = yID;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string wTeaser { get; set; }
        public string wUrl { get; set; }
        public string yUrl { get; set; }
        public string yID { get; set; }
    }
}
