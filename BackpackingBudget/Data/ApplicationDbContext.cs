using System;
using System.Threading;
using BackpackingBudget.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackpackingBudget.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Budget> Budget { get; set; }
        public DbSet<BudgetCategory> BudgetCategory { get; set; }
        public DbSet<BudgetItem> BudgetItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BudgetItem>().Property<decimal>("Cost")
                        .HasColumnType("money");
            modelBuilder.Entity<BudgetCategory>().Property<decimal>("BudgetPerDay")
                        .HasColumnType("money");
            modelBuilder.Entity<Budget>().Property<decimal>("BudgetAmount")
                        .HasColumnType("money");

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Brian",
                LastName = "Jobe",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);


            modelBuilder.Entity<Budget>().HasData(
                new Budget
                {
                    BudgetId = 1,
                    Name = "Peru",
                    StartDate = new DateTime(2019, 08, 01),
                    EndDate = new DateTime(2019, 08, 15),
                    UserId = user.Id,
                    BudgetAmount = 1400
                });

            modelBuilder.Entity<BudgetCategory>().HasData(
                new BudgetCategory
                {
                    BudgetCategoryId = 1,
                    BudgetId = 1,
                    BudgetPerDay = 20,
                    Name = "Transportation"
                },
                new BudgetCategory
                {
                    BudgetCategoryId = 2,
                    BudgetId = 1,
                    BudgetPerDay = 20,
                    Name = "Food"
                },
                new BudgetCategory
                {
                    BudgetCategoryId = 3,
                    BudgetId = 1,
                    BudgetPerDay = 20,
                    Name = "Lodging"
                },
                new BudgetCategory
                {
                    BudgetCategoryId = 4,
                    BudgetId = 1,
                    BudgetPerDay = 20,
                    Name = "Activities"
                },
                new BudgetCategory
                {
                    BudgetCategoryId = 5,
                    BudgetId = 1,
                    BudgetPerDay = 20,
                    Name = "Misc"
                });


        }
    }
}
