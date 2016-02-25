namespace MovieMind.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Common.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using SentimentAnalyzer.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

            Database.SetInitializer<ApplicationDbContext>(null);

            //var objectContext = (this as IObjectContextAdapter).ObjectContext;

            //objectContext.CommandTimeout = 1 * 60 * 60; // 1 hour timeout as the SentimentAnalyzer makes big queries
        }

        public IDbSet<Movie> Movies { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Genre> Genres { get; set; }

        public IDbSet<Language> Languages { get; set; }

        public IDbSet<MoviePerson> MoviePeople { get; set; }

        public IDbSet<Review> Reviews { get; set; }

        public IDbSet<SentimentReview> TrainigReviews { get; set; }

        public IDbSet<SentimentTestingReview> TestingReviews { get; set; }

        public IDbSet<SentimentCategory> ReviewCategories { get; set; }

        public IDbSet<SentimentWord> VocabularyWords { get; set; }

        public IDbSet<ReviewWord> ReviewWords { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();

            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                var errors = (ex as System.Data.Entity.Validation.DbEntityValidationException)
                         .EntityValidationErrors
                         .ToList()[0]
                         .ValidationErrors
                         .ToList()[0];

                throw;
            }
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
