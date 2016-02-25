namespace MovieMind.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using MovieMind.Data.Common;
    using MovieMind.Data.Models;
    using Web;

    public class ReviewsService : IReviewsService
    {
        private readonly IDbRepository<Review> reviews;
        private readonly IIdentifierProvider identifierProvider;

        public ReviewsService(
            IIdentifierProvider identifierProvider,
            IDbRepository<Review> reviewsRepo)
        {
            this.identifierProvider = identifierProvider;
            this.reviews = reviewsRepo;
        }

        public int Edit(Review review)
        {
            this.reviews.Update(review);
            this.reviews.Save();

            return review.Id;
        }

        public int Create(Review review)
        {
            this.reviews.Add(review);
            this.reviews.Save();

            return review.Id;
        }

        public void Delete(Review review)
        {
            this.reviews.Delete(review);
            this.reviews.Save();
        }

        public IQueryable<Review> GetAll()
        {
            return this.reviews.All();
        }

        public IQueryable<Review> GetPage(int page, int size)
        {
            return this.reviews.All()
                .OrderByDescending(r => r.CreatedOn)
                .ThenByDescending(r => r.Rating)
                .Skip((page - 1) * size)
                .Take(size);
        }

        public IQueryable<Review> GetByUserId(string userId)
        {
            return this.reviews.All()
                .Where(r => r.AuthorId == userId);
        }

        public IQueryable<Review> GetByMovieId(string movieId)
        {
            var id = this.identifierProvider.DecodeId(movieId);

            return this.reviews.All()
                .Where(r => r.MovieId == id);
        }

        public Review GetById(int id)
        {
            return this.reviews.GetById(id);
        }
    }
}
