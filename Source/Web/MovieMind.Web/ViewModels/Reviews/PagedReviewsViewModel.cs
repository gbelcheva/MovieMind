namespace MovieMind.Web.ViewModels.Reviews
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using MovieMind.Data.Models;
    using MovieMind.Web.Infrastructure.Mapping;

    public class PagedReviewsViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
