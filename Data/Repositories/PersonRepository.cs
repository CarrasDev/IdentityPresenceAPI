using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PersonRepository : IRepository<PersonEntity, Guid>
    {
        private readonly AplicationDbContext _context;

        public PersonRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        }






    }
}
