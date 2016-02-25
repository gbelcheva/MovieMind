namespace MovieMind.Web.Areas.Administration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using MovieMind.Data.Models;
    using MovieMind.Web.Infrastructure.Mapping;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string MovieName { get; set; }

        public string AuthorName { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>().
                ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName)).
                ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Title));
        }
    }
}