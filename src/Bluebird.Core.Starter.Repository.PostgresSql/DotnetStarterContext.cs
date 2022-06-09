using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bluebird.Core.Starter.Domain.Models;
using System;

namespace Bluebird.Core.Starter.Repository.PostgresSql
{
    public class DotnetStarterContext : IdentityDbContext
    {
        public DotnetStarterContext(DbContextOptions<DotnetStarterContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DotnetStarterContext).Assembly);

            #region Seed Admin
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "473b3bf9-6511-4c49-a886-af6b9859e65c",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAEAACcQAAAAEE3C9XqMd0HeNumy4Y8NRtG2Cia68T7trckJuDBgTh+0iZe28tmfkwe26SPVmcAGDg==",
                SecurityStamp = "GIZJIFOSJRP23ZNSNK5MNYJOTUV34V7O",
                ConcurrencyStamp = "5a92dd5c-6243-4d3a-88b3-ffb642985c0d",
                LockoutEnabled = true,
            });

            modelBuilder.Entity<Movie>().HasData(new Movie
            {
                Id = Guid.Parse("384c70dd-4800-42df-9383-2c4f26fb98e8"), 
                Genre = Enums.Genre.HORROR,
                Name = "The Fly",
                Director = "David Cronenburg",
                ReleaseYear = "1986"
            }, new Movie
            {
                Id = Guid.Parse("02cb57b2-c4cf-4347-9c26-c5c2455c66a6"),
                Genre = Enums.Genre.COMEDY,
                Name = "Ace Ventura: Pet Detective",
                Director = "Tom Shadyac",
                ReleaseYear = "1994"
            }, new Movie
            {
                Id = Guid.Parse("d7557141-e76c-46df-ae2a-4972e9117b68"),
                Genre = Enums.Genre.COMEDY,
                Name = "Back to the Future",
                Director = "Robert Zemeckis",
                ReleaseYear = "1985"
            }, new Movie
            {
                Id = Guid.Parse("d1d6078d-cfc7-42b2-bd17-6f8ead40c049"),
                Genre = Enums.Genre.ACTION,
                Name = "Die Hard",
                Director = "John McTiernan",
                ReleaseYear = "1988"
            }, new Movie
            {
                Id = Guid.Parse("e1a55be3-cc50-4196-a870-befe720600c4"),
                Genre = Enums.Genre.DRAMA,
                Name = "Titanic",
                Director = "James Cameron",
                ReleaseYear = "1997"
            });
            #endregion
        }
        public DbSet<Movie> Movies { get; set; }
    }
}
