using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class VisitRepository : IRepository<VisitEntity, Guid>,
                                   IVisitRepository<VisitEntity>
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



        // *******************************
        // IVisitRepository implementation
        // *******************************

        public async Task<bool> HasActiveVisitAsync(Guid personId)
        {
            return await _context.Visits
                        .AnyAsync(v => v.PersonId == personId && v.ExitTime == null);   // Busca la existencia del primer elemento sin recorrer todos los campos de la tabla.
        }

        public async Task<VisitEntity?> GetActiveVisitByPersonCodeAsync(string personCode)
        {
            return await _context.Visits
                        .Include(v => v.Person)
                        .FirstOrDefaultAsync(v => v.Person != null
                                                  && v.Person.Code.ToUpper() == personCode.ToUpper()
                                                  && v.ExitTime == null);
        }

        public async Task<IEnumerable<VisitEntity>> GetActiveVisitsAsync()
        {
            return await _context.Visits
                        .Include(v => v.Person)
                        .Where(v => v.ExitTime == null)
                        .OrderBy(v => v.EntryTime)
                        .ToListAsync();
        }

        public async Task<IEnumerable<VisitEntity>> GetVisitsByPersonIdAsync(Guid personId)
        {
            return await _context.Visits
                        .Include(v => v.Person)
                        .Where(v => v.PersonId == personId)
                        .OrderByDescending(v => v.EntryTime)
                        .ToListAsync();
        }

    }
}
