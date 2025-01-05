﻿using BankingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class BankingDbContext : IdentityDbContext<ApplicationUser>
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DebitCard>()
                .HasOne(x => x.Owner)
                .WithOne(x => x.DebitCard)
                .HasForeignKey<DebitCard>(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        public DbSet<DebitCard> DebitCards { get; set; }
    }
}