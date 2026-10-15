using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class VisitRepository : IRepository<VisitEntity, Guid>
    {
        private readonly ApplicationDbContext _context;

        public VisitRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<VisitEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Visits
                        .Include(v => v.Person)
                        .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VisitEntity>> GetAllAsync()
        {
            return await _context.Visits
                        .Include(v => v.Person)
                        .AsNoTracking()
                        .OrderByDescending(v => v.EntryTime)
                        .ToListAsync();
        }

        public async Task AddAsync(VisitEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Visits.AddAsync(entity);
        }        

        public Task UpdateAsync(VisitEntity entity)                     //Asincronia simulada. UpdateAsync no es realmente asincrono.
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Visits.Update(entity);
            return Task.CompletedTask;                                  // Importante indicar que la tarea ha finalizado.
        }

        public Task DeleteAsync(VisitEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Visits.Remove(entity);
            return Task.CompletedTask;                                  // Importante indicar que la tarea ha finalizado.
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }





    }
}
