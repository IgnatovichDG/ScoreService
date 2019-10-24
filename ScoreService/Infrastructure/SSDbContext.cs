using Microsoft.EntityFrameworkCore;
using ScoreService.Models;

namespace ScoreService.Infrastructure
{
    public class SSDbContext : DbContext
    {
        public SSDbContext(DbContextOptions<SSDbContext> options) : base(options)
        {
            
        }



        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}