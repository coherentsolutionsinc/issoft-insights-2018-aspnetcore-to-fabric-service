using Microsoft.EntityFrameworkCore;

namespace JokesApi.Impl.Services.EntityFramework
{
    public class DbJokesContext : DbContext
    {
        public DbSet<DbJokeEntity> Jokes { get; set; }

        public DbJokesContext(
            DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbJokeEntity>(
                builder =>
                {
                    builder.HasKey(e => e.Id);

                    builder.Property(e => e.Id).ValueGeneratedNever();
                });
        }
    }
}