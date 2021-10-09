using BaseSwagger.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseSwagger.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Dummy> Dummies { get; set; }
    }
}
