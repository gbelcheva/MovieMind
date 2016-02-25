namespace MovieMind.Web.ViewModels.Movies
{
    using Data.Models;
    using Services.Web;
    using Web.Infrastructure.Mapping;

    public class MovieViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? Year { get; set; }

        public string Poster { get; set; }

        public string EncodedId
        {
            get
            {
                IIdentifierProvider identifier = new IdentifierProvider();
                return identifier.EncodeId(this.Id);
            }
        }

        public string Url
        {
            get
            {
                return string.Format("Movie/{0}-{1}-{2}", this.EncodedId, this.Title.Replace(" ", "-"), this.Year);
            }
        }
    }
}
