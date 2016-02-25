namespace Tools.SentimentAnalyzer.AnalysisModels
{
    using System.Collections.Generic;

    public class Category
    {
        public string Name { get; set; }

        public int TrainSampleSize { get; set; }

        public Dictionary<string, int> WordOccurrences { get; set; }
    }
}
