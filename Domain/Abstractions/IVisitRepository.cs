using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IVisitRepository<TEntity> where TEntity : class
    {
        Task<bool> HasActiveVisitAsync(Guid personId);

        Task<IEnumerable<TEntity>> GetActiveVisitsAsync();

        Task<IEnumerable<TEntity>> GetVisitsByPersonIdAsync(Guid personId);
    }
}
