using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class GreetingContext : DbContext
    {
        public GreetingContext(DbContextOptions<GreetingContext> options) : base(options) { }

        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<GreetingEntity> GreetMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GreetingEntity>()
                        .HasOne(g => g.User)
                        .WithMany(u => u.Greetings)
                        .HasForeignKey(g => g.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
