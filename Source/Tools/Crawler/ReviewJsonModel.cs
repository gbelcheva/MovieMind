namespace Crawler
{
    using Newtonsoft.Json;

    public class ReviewJsonModel
    {
        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
