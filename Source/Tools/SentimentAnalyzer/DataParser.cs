namespace Tools.SentimentAnalyzer
{
    using System.Collections.Generic;
    using System.IO;
    using AnalysisModels;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using global::SentimentAnalyzer.Data.Models;
    using global::SentimentAnalyzer.Services.Data;
    using Properties;
    using System.Text;
    using System;
    public class DataParser
    {
        public static IList<JsonReviewModel> ParseJson(string jsonPath)
        {
            var jsonString = File.ReadAllText(jsonPath);
            JObject reviews = JObject.Parse(jsonString);

            var results = JsonConvert.DeserializeObject<List<JsonReviewModel>>(jsonString);

            foreach (var res in results)
            {
                res.Rating = double.Parse(res.RatingString.Split('/')[0]);
            }

            return results;
        }

        public static void ParseVocabularyToDb(IWordsService words)
        {
            byte[] vocabularyBytes = Resources.vocabulary;
            string vocabularyString = Encoding.UTF8.GetString(vocabularyBytes, 0, vocabularyBytes.Length);
            string[] vocabularyLines = vocabularyString.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in vocabularyLines)
            {
                var lineParts = line.Split(new string[] { "\t"}, StringSplitOptions.RemoveEmptyEntries);

                if (lineParts.Length != 2)
                {
                    continue;
                }

                words.Create(new SentimentWord()
                {
                    Word = lineParts[0].Replace("_", " "),
                    Polarity = int.Parse(lineParts[1])
                });
            }
        }
    }
}
