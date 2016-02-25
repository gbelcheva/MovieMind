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

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string Content { get; set; }

        public string SanitizedContent
        {
            get
            {
                var sanitizer = new HtmlSanitizer();
                return sanitizer.Sanitize(this.Content);
            }
        }

        public double Rating { get; set; }

        public MovieDetailsViewModel Movie { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));
        }
    }
}
