using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Last.Bench.Coder.Beauty.World.Entity;
using Microsoft.EntityFrameworkCore;

namespace Last.Bench.Coder.Beauty.World.DataContext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<Store> Store { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Service> Service { get; set; }
         public DbSet<ServiceBanner> ServiceBanner { get; set; }
    }
}