using Microsoft.EntityFrameworkCore;
using SomaliskDanskForening_Lib.Models;
using System;

namespace SomaliskDanskForening_Lib.Data
{
    public class ForeningDbContext : DbContext
    {
        private const string ConnStr = "Server=(localdb)\\mssqllocaldb;Database=ForeningDB;Trusted_Connection=True";
        public ForeningDbContext(DbContextOptions<ForeningDbContext> options) : base(options)
        {
        }
        public DbSet<Donation> Donations => Set<Donation>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Event> Events => Set<Event>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Netværksmøde med Lokale Foreninger",
                    Date = new DateTime(2025, 1, 18),
                    Duration = 2,
                    Description = "Mød repræsentanter fra lokale danske foreninger og styrk samarbejdet.",
                    StartTime = 18
                }
         

            );
        }
    }
}
