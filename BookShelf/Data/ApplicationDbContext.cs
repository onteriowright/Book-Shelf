using System;
using System.Collections.Generic;
using System.Text;
using BookShelf.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 1,
                Name = "Technology"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 2,
                Name = "Self Improvement"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 3,
                Name = "Sports"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 4,
                Name = "Arts and Crafts"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 5,
                Name = "Financial Literacy"
            });


            //If you name your foreign keys correctly, then you don't need this.
            modelBuilder.Entity<BookGenre>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.ListOfBookGenres)
                .HasForeignKey(pt => pt.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(pt => pt.Genre)
                .WithMany(t => t.BookGenres)
                .HasForeignKey(pt => pt.GenreId);
        }
    }
}
