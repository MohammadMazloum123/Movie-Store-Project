using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieMVC.Models.Domain
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) {

        }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
    }
}
