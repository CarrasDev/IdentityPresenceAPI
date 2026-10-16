using Application.UseCases.Persons;
using Application.UseCases.Visits;
using Data;
using Data.Repositories;
using Domain;
using Domain.Abstractions;
using WebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// En AddData() se registran los repositorios de la capa de datos, para que puedan ser inyectados en los casos de uso.
// Se ha implementado haciendo uso de la extensión de métodos, Usando el paquete Microsoft.Extensions.DependencyInjection.Abstractions,
// que permite agregar servicios a la colección de servicios de la aplicación.
// De esta manera podemos centralizar las dependencias de la capa de datos en un solo lugar.
builder.Services.AddData(connectionString);
// builder.Services.AddScoped<IRepository<PersonEntity, Guid>, PersonRepository>();
// builder.Services.AddScoped<ICodeRepository<PersonEntity>, PersonRepository>();


builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<GetPersonByIdUseCase>();
builder.Services.AddScoped<GetPersonByCodeUseCase>();
builder.Services.AddScoped<GetAllPersonsUseCase>();
builder.Services.AddScoped<UpdatePersonUseCase>();
builder.Services.AddScoped<DeletePersonUseCase>();

builder.Services.AddScoped<GetActiveVisitsUseCase>();
builder.Services.AddScoped<GetAllVisitsUseCase>();
builder.Services.AddScoped<GetVisitsByPersonUseCase>();
builder.Services.AddScoped<RegisterEntryUseCase>();
builder.Services.AddScoped<RegisterExitUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPersonsEndpoints();

app.Run();