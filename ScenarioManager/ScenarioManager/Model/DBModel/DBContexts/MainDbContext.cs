using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel.DBContexts
{
    public class MainDbContext:DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasOne(p => p.ParentGroup)
                .WithMany(t => t.ChildrenGroups)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                 .HasOne(p => p.UserGroup)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Scenario>()
                .HasOne(p => p.UserGroup)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Scenario>()
              .HasOne(p => p.Author)
              .WithMany()
              .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
