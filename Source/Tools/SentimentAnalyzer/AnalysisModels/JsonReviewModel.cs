namespace Tools.SentimentAnalyzer.AnalysisModels
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonReviewModel
    {
        public double Rating { get; set; }

        [JsonProperty("rating")]
        public string RatingString { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        public List<string> Words { get; set; }
    }
}
