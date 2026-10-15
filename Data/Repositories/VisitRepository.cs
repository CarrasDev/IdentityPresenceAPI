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






    }
}
