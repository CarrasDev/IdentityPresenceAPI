using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PersonRepository : IRepository<PersonEntity, Guid>,
                                    ICodeRepository<PersonEntity>
    {
        private readonly AplicationDbContext _context;

        public PersonRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PersonEntity>> GetAllAsync()
        {
            return await _context.Persons
                .AsNoTracking()                 // TODO: Sin traking mejora el rendimiento de las consultas de solo lectura al evitar el seguimiento de cambios en las entidades recuperadas.
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToListAsync();
        }

        public async Task AddAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Persons.AddAsync(entity);
        }

        // TODO: _context.Persons.Update(entity) es sincrono en EF, con lo cual trabajamos simulando asincronia haciendo uso de return Task.CompletedTask.
        public Task UpdateAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Persons.Update(entity);  // no podemos usar await en este punto por no ser un metodo asincrono, EF Core maneja la actualización de manera sincrona.
            return Task.CompletedTask;

            // NOTA:    Update(entity) solamente marca la entidad como modificada en el CangeTracker de EF por este motivo no tiene necesidad de ser asincrono.
            //          EF Core no realiza la actualización en la base de datos hasta que se llama a SaveChangesAsync() en el contexto, lo cual es asincrono.

        }

        // Misma tesitura que en a función anterior, EF Core maneja la eliminación de manera sincrona, por lo que no podemos usar await en este punto.
        public Task DeleteAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Persons.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }



        // ******************************
        // ICodeRepository implementation
        // ******************************

        public async Task<PersonEntity?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El código no puede estar vacío", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();

            return await _context.Persons.FirstOrDefaultAsync(p => p.Code == normalizedCode);
        }

        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El código no puede estar vacío", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();

            return await _context.Persons.AnyAsync(p => p.Code == normalizedCode);
        }
    }
}
