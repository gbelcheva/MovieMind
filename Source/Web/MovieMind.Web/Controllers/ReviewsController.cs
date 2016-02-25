namespace MovieMind.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models;
    using Infrastructure.Mapping;
    using Microsoft.AspNet.Identity;
    using SentimentAnalyzer.Services.Data;
    using Services.Data;
    using Services.Web;
    using Tools.SentimentAnalyzer;
    using ViewModels.Reviews;

    public class ReviewsController : BaseController
    {
        private const int ReviewsPerPage = 4;
        private readonly IReviewsService reviews;
        private readonly IUsersService users;
        private readonly IMoviesService movies;
        private readonly ICategoriesService sentimentCategories;
        private readonly ITrainingReviewsService trainingReviews;
        private readonly IWordsService sentmentWords;
        private readonly ReviewAnalyzer reviewAnalyzer;

        public ReviewsController(
            IReviewsService reviews,
            IUsersService users,
            IMoviesService movies,
            ICategoriesService sentimentCategories,
            ITrainingReviewsService trainingReviews,
            IWordsService sentmentWords)
        {
            this.reviews = reviews;
            this.sentimentCategories = sentimentCategories;
            this.sentmentWords = sentmentWords;
            this.trainingReviews = trainingReviews;
            this.users = users;
            this.movies = movies;

            if (!this.sentimentCategories.Any())
            {
                this.reviewAnalyzer = new ReviewAnalyzer(this.sentmentWords, this.sentimentCategories, this.trainingReviews);
            }
            else
            {
                this.reviewAnalyzer = new ReviewAnalyzer(this.sentmentWords, this.sentimentCategories);
            }
        }

        [HttpGet]
        public PartialViewResult ReviewsByMovieId(string id)
        {
            var allReviews = this.reviews
                .GetByMovieId(id);

            var totalReviews = this.reviews.GetAll().Count();
            int totalPages = (int)Math.Ceiling(totalReviews / (decimal)ReviewsPerPage);

            var reviewsList = allReviews
                .Take(ReviewsPerPage)
                .To<ReviewViewModel>()
                .ToList();

            var result = new PagedReviewsViewModel()
            {
                CurrentPage = 1,
                TotalPages = totalPages,
                Reviews = reviewsList
            };

            return this.PartialView("_MovieById", result);
        }

        [HttpPost]
        public PartialViewResult ReviewsByMovieId(string id, int page = 1)
        {
            var allReviews = this.reviews
                .GetByMovieId(id)
                .Skip((page - 1) * ReviewsPerPage)
                .Take(ReviewsPerPage)
                .To<ReviewViewModel>()
                .ToList();

            var totalReviews = this.reviews.GetAll().Count();
            int totalPages = (int)Math.Ceiling(totalReviews / (decimal)ReviewsPerPage);

            var viewModel = new PagedReviewsViewModel()
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Reviews = allReviews
            };

            return this.PartialView("_MovieReviews", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostReview(ReviewPostModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var reviewContent = model.Content;
            var rating = this.reviewAnalyzer.AnalyzeReview(reviewContent);

            string currentUserId = this.User.Identity.GetUserId();
            ApplicationUser currentUser = this.users.GetAll().FirstOrDefault(x => x.Id == currentUserId);

            var currentMovie = this.movies.GetById(model.MovieId);

            this.reviews.Create(new Review()
            {
                Author = currentUser,
                AuthorId = currentUserId,
                Content = model.Content.Replace(Environment.NewLine, "<br />"),
                Rating = double.Parse(rating),
                Movie = currentMovie
            });

            IIdentifierProvider identifier = new IdentifierProvider();

            this.TempData["Notification"] = "The review was submitted successfully.";

            return this.View();
        }
    }
}
