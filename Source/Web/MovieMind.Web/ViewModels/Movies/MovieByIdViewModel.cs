namespace MovieMind.Web.ViewModels.Movies
{
    using System.Collections.Generic;
    using MovieMind.Data.Models;
    using MovieMind.Services.Web;
    using MovieMind.Web.Infrastructure.Mapping;
    using Reviews;

    public class MovieByIdViewModel
    {
        public MovieDetailsViewModel Movie { get; set; }

        public ReviewPostModel PostReview { get; set; }

        public PagedReviewsViewModel Reviews { get; set; }

        public string EncodedId
        {
            get
            {
                IIdentifierProvider identifier = new IdentifierProvider();
                return identifier.EncodeId(this.Movie.Id);
            }
        }
    }
}
