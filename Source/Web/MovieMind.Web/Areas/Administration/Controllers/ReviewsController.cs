namespace MovieMind.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Mapping;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using MovieMind.Data.Models;
    using MovieMind.Services.Data;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviews;

        public ReviewsController(IReviewsService reviews)
        {
            this.reviews = reviews;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Reviews_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = this.reviews.GetAll()
                .To<ReviewViewModel>()
                .ToDataSourceResult(request);

            return this.Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reviews_Create([DataSourceRequest]DataSourceRequest request, Review review)
        {
            if (this.ModelState.IsValid)
            {
                var entity = new Review
                {
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedOn = review.CreatedOn,
                    ModifiedOn = review.ModifiedOn,
                    IsDeleted = review.IsDeleted,
                    DeletedOn = review.DeletedOn
                };

                this.reviews.Create(entity);
                review.Id = entity.Id;
            }

            return this.Json(new[] { review }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reviews_Update([DataSourceRequest]DataSourceRequest request, Review review)
        {
            if (this.ModelState.IsValid)
            {
                var entity = new Review
                {
                    Id = review.Id,
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedOn = review.CreatedOn,
                    ModifiedOn = review.ModifiedOn,
                    IsDeleted = review.IsDeleted,
                    DeletedOn = review.DeletedOn
                };

                this.reviews.Edit(entity);
            }

            return this.Json(new[] { review }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reviews_Destroy([DataSourceRequest]DataSourceRequest request, Review review)
        {
            this.reviews.Delete(review);

            return this.Json(new[] { review }.ToDataSourceResult(request, ModelState));
        }
    }
}
