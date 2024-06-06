using Application.Interfaces;
using Domain.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DatabaseContext
{
    public class ApplicationContext : IdentityDbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Category> Categories { get; set; }

        void IApplicationContext.SaveChanges() => this.SaveChanges();
    }
}
