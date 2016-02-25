namespace MovieMind.Web.ViewModels.Movies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using MovieMind.Data.Models;
    using MovieMind.Services.Web;
    using MovieMind.Web.Infrastructure.Mapping;

    public class MovieDetailsViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double Rating { get; set; }

        public int? Year { get; set; }

        public string Poster { get; set; }

        public string Plot { get; set; }

        public string Runtime { get; set; }

        public string Awards { get; set; }

        public double? ImdbRating { get; set; }

        public int? ImdbVotes { get; set; }

        public IEnumerable<string> Director { get; set; }

        public IEnumerable<string> Writer { get; set; }

        public IEnumerable<string> Actors { get; set; }

        public IEnumerable<string> Language { get; set; }

        public IEnumerable<string> Country { get; set; }

        public IEnumerable<string> Genre { get; set; }

        public string EncodedId
        {
            get
            {
                IIdentifierProvider identifier = new IdentifierProvider();
                return identifier.EncodeId(this.Id);
            }
        }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Movie, MovieDetailsViewModel>()
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director.Select(d => d.FullName)))
                .ForMember(dest => dest.Writer, opt => opt.MapFrom(src => src.Writer.Select(w => w.FullName)))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors.Select(a => a.FullName)))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Select(g => g.Name)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Select(l => l.Name)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Select(c => c.Name)))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Select(c => c.Rating).Sum() / src.Reviews.Count()));
        }
    }
}
