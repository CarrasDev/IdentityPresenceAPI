using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }

        public DbSet<PersonEntity> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");
                
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Ignore(e => e.FullName);

                entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }



    }
}
