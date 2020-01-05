using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 7, Name = "Mark", Department = "HR", Email = "blabla@gmail.com"
                }
                );
                */
        }

    }

    
}
