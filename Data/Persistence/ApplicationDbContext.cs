using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<VisitEntity> Visits { get; set; }

        // Definimos como EF Core construye la tabla en BBDD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);             // Recomendado llamar siempre a la implementación base de OnModelCreating para asegurarse de que cualquier
                                                            // configuración predeterminada de EF Core se aplique correctamente.


            // Configuración de la entidad PersonEntity
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");
                
                entity.HasKey(e => e.Id);               // PK

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


            // Configuración de la entidad VisitEntity
            modelBuilder.Entity<VisitEntity>(entity =>
            {
                entity.ToTable("Visits");

                entity.HasKey(e => e.Id);                   // PK

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PersonId)
                    .IsRequired();

                entity.Property(e => e.EntryTime)
                    .IsRequired();

                entity.Property(e => e.ExitTime)
                    .IsRequired(false);

                entity.Ignore(e => e.isActive);
                entity.Ignore(e => e.Duration);

                entity.HasOne(e => e.Person)                // 1 visita pertenece a 1 persona, pero una persona puede tener muchas visitas, por eso se usa WithMany()
                    .WithMany()
                    .HasForeignKey(e => e.PersonId)         // La clave foránea es PersonId en la tabla Visits
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.PersonId);
                entity.HasIndex(e => e.EntryTime);
                entity.HasIndex(e => new {e.PersonId, e.ExitTime});  // Índice compuesto para consultas de visitas activas por persona

                entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            });



        }


        // Definimos qué pasa justo antes de guardar en la tabla de BBDD.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Metadata.FindProperty("UpdateAt") != null)
                {
                    entry.Property("UpdateAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }


    }
}
