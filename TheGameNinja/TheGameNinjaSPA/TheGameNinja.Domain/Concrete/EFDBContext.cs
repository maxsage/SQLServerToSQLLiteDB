using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGameNinja.Domain.Entities;
using TheGameNinja.Domain;
namespace TheGameNinja.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        //TheGameNinjaRepository repo = new TheGameNinjaRepository();

        public DbSet<Videogame> Videogames { get; set; }
        public DbSet<Accolade> Accolades { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFDBContext>(null);

            base.OnModelCreating(modelBuilder);
        }
    }
}
