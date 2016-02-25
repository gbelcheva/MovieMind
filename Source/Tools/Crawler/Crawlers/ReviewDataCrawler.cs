namespace Crawler.Crawlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using AngleSharp;
    using MovieMind.Common;
    using Newtonsoft.Json;
    using SentimentAnalyzer.Data.Models;
    using SentimentAnalyzer.Services.Data;

    public class ReviewDataCrawler
    {
        private const int StartCollection = 288; // 0;
        private const int EndCollection = 425;
        private const string StatusMessage = "Scraping reviews collection {0}/{1} (x100 reviews)";
        private const string RatingRegex = @"(.\d+\.?\d*)\/(\d+\.?\d*)|(\d+\.?\d*)\/(\d+\.?\d*)"; // matches 4/5, 4.5/5, and .5/5 rating formats
        private const string FinishedMessage = "\nFinished!\nTotal Rated Reviews: {0}\n";
        private const string RatedReviewsPath = "../../rated-reviews.json";
        private const string UnratedReviewsPath = "../../unrated-reviews.json";
        private const string ReviewUrlPattern = "http://www.imdb.com/reviews/{0}/{0}{1}.html";
        private readonly ITrainingReviewsService trainingReviews;

        public ReviewDataCrawler(ITrainingReviewsService trainingReviews)
        {
            this.trainingReviews = trainingReviews;
        }

        public static void ScrapeDataToJson()
        {
            List<ReviewJsonModel> ratedReviews = new List<ReviewJsonModel>();
            List<ReviewJsonModel> unratedReviews = new List<ReviewJsonModel>();

            var configuration = Configuration.Default.WithDefaultLoader();
            var browsingContext = BrowsingContext.New(configuration);

            for (int i = StartCollection; i <= EndCollection; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(
                    StatusMessage,
                    i - StartCollection + 1,
                    EndCollection - StartCollection + 1);

                for (int j = 0; j <= 99; j++)
                {
                    var collection = i.ToString().PadLeft(2, '0');
                    var review = j.ToString().PadLeft(2, '0');

                    var curUrl = string.Format(ReviewUrlPattern, collection, review);
                    var document = browsingContext.OpenAsync(curUrl).Result;
                    var allElements = document.All;
                    StringBuilder content = new StringBuilder();
                    string rating = string.Empty;

                    foreach (var element in allElements)
                    {
                        if (element.TagName.ToLower() == "pre" && element.InnerHtml.Contains("X-RT-RatingText"))
                        {
                            rating = element.InnerHtml.Split(new string[] { "X-RT-RatingText:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            rating = Regex.Match(rating, RatingRegex).Value;
                            if (rating == string.Empty)
                            {
                                continue;
                            }

                            ratedReviews.Add(new ReviewJsonModel()
                            {
                                Rating = rating,
                                Content = content.ToString()
                            });

                            content.Clear();
                            break;
                        }
                        else if (element.TagName.ToLower() == "p" && element.ChildElementCount == 0)
                        {
                            content.Append(element.InnerHtml + "\n");
                        }
                    }

                    if (content.Length > 0)
                    {
                        unratedReviews.Add(new ReviewJsonModel()
                        {
                            Rating = "none",
                            Content = content.ToString()
                        });

                        content.Clear();
                    }
                }
            }

            string json = JsonConvert.SerializeObject(ratedReviews.ToArray());
            File.WriteAllText(RatedReviewsPath, json);
            json = JsonConvert.SerializeObject(unratedReviews.ToArray());
            File.WriteAllText(UnratedReviewsPath, json);

            Console.WriteLine(FinishedMessage, ratedReviews.Count, unratedReviews.Count);
        }

        public void ScrapeDataToDb()
        {
            var configuration = Configuration.Default.WithDefaultLoader();
            var browsingContext = BrowsingContext.New(configuration);

            int trainingReviewsCount = 0;

            for (int i = StartCollection; i <= EndCollection; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(
                    StatusMessage,
                    i - StartCollection + 1,
                    EndCollection - StartCollection + 1);

                for (int j = 0; j <= 99; j++)
                {
                    var collection = i.ToString().PadLeft(2, '0');
                    var review = j.ToString().PadLeft(2, '0');

                    var curUrl = string.Format(ReviewUrlPattern, collection, review);
                    var document = browsingContext.OpenAsync(curUrl).Result;
                    var allElements = document.All;
                    StringBuilder content = new StringBuilder();
                    string rating = string.Empty;

                    foreach (var element in allElements)
                    {
                        if (element.TagName.ToLower() == "pre" && element.InnerHtml.Contains("X-RT-RatingText"))
                        {
                            rating = element.InnerHtml.Split(new string[] { "X-RT-RatingText:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            rating = Regex.Match(rating, RatingRegex).Value;

                            if (rating == string.Empty || content.Length < 1)
                            {
                                continue;
                            }

                            rating = Regex.Replace(rating, "^[.]", "0."); // deal with .5 type values

                            var currentRating = double.Parse(rating.Split('/')[0]);
                            var maxRating = double.Parse(rating.Split('/')[1]);

                            if (currentRating < 1)
                            {
                                currentRating++;  // adjust zero ratings
                                maxRating++;
                            }

                            currentRating = currentRating * GlobalConstants.RatingSystemPoints / maxRating; // normalization for rating system
                            currentRating = Math.Round(currentRating * 2, MidpointRounding.AwayFromZero) / 2; // round to halves

                            this.trainingReviews.Create(new SentimentReview()
                            {
                                Rating = currentRating,
                                Content = content.ToString()
                            });

                            trainingReviewsCount++;
                            content.Clear();
                            break;
                        }
                        else if (element.TagName.ToLower() == "p" && element.ChildElementCount == 0)
                        {
                            content.Append(element.InnerHtml + "\n");
                        }
                    }

                    /*if (content.Length > 0)
                    {
                        unratedReviews.Add(new ReviewJsonModel()
                        {
                            Rating = "none",
                            Content = content.ToString()
                        });

                        content.Clear();
                    }*/
                }
            }

            Console.WriteLine(FinishedMessage, trainingReviewsCount);
        }
    }
}
