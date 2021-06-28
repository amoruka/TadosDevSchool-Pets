using Microsoft.EntityFrameworkCore;
using Pets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pets.Persistence
{
    public class PetsContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }

        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Food> Foods { get; set; }

        public PetsContext(DbContextOptions<PetsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cat>();
            builder.Entity<Dog>();

            base.OnModelCreating(builder);
        }
    }
}
