using FP200OK01.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Utilities
{
    class MovieContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<IMDBData> IMDBData { get; set; }
        public DbSet<Review> Review { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FinalProject_200DB");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasKey(c => new { c.UserId, c.MovieId });

        }
    }
}