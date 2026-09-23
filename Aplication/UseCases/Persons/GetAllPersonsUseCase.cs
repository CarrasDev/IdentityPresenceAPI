using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.UseCases.Persons
{
    public class GetAllPersonsUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _Repository;

        public GetAllPersonsUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _Repository = repository;
        }

        public async Task<IEnumerable<PersonEntity>> ExecuteAsync()
        {
            return await _Repository.GetAllAsync();
        }
    }
}
