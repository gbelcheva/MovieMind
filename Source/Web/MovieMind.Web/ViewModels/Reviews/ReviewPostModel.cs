namespace MovieMind.Web.ViewModels.Reviews
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using AutoMapper;
    using Ganss.XSS;
    using MovieMind.Data.Models;
    using MovieMind.Web.Infrastructure.Mapping;
    using Movies;

    public class ReviewPostModel
    {
        [Required]
        [StringLength(9999999, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 100)]
        public string Content { get; set; }

        public string MovieId { get; set; }
    }
}
