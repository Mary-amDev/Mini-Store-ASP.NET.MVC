using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;   // call EntityFrameWork 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mini_store.Models;

namespace mini_store.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser> // EntityFramework
    {
            public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
            {

            }


        public DbSet<Product> products{get;set;}
        // public DbSet<Items> items{get;set;}
        // public DbSet<Costumers> costumers{get;set;}
        // public DbSet<Details> details{get;set;}
        public DbSet<Categories> categories{get;set;}

        public DbSet<Images> images {get; set;}


    }


}