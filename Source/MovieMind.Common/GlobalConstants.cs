namespace MovieMind.Common
{
    using System.Collections.Generic;

    public class GlobalConstants
    {
        public const int RecommendedMoviesCount = 20;
        public const int DefaultPageSize = 5;
        public const int MaxPageSize = 5;
        public const int DefaultTrendingCount = 12;
        public const int DefaultTrendingCacheDays = 7;

        public const int RatingSystemPoints = 10;

        public const string AdministratorRoleName = "Administrator";
        public const string DefaultPosterUrl = "http://www.makeupstudio.lu/html/images/poster/no_poster_available.jpg";
        public static readonly int[] AgeGroups = new int[] { 18, 29, 44 };
    }
}
