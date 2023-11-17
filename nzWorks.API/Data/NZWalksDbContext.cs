using Microsoft.EntityFrameworkCore;
using nzWorks.API.Models.Domain;

namespace nzWorks.API.Data
{
    public class NZWalksDbContext :DbContext
    {
        //create constructor
        public NZWalksDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }
    }
}
