using Application.DTOs.Person;
using Application.UseCases.Persons;
using Microsoft.Identity.Client.NativeInterop;

namespace WebApi.EndPoints
{
    public static class PersonsEndpoints
    {
        public static void MapPersonsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/persons")
                .WithTags("Persons");

            group.MapGet("/{id:guid}", async(Guid id, GetPersonByIdUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(id);
                    return Results.Ok(person);                              // Status 200 OK
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });    // Status 404 Not Found
                }
            }).WithName("GetPersonById")
              .WithSummary("Gets a person by their unique identifier.")
              .WithDescription("Retrieves the details of a person using their unique identifier (GUID). Returns 200 if the person is found, or 404 if not found or not existing.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound);


            group.MapPost("/", async (CreatePersonDto dto, CreatePersonUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/persons/{person.Id}", person);    // Status 201 Created
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });          // Status 400 Bad Request
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });          // Status 400 Bad Request
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);                 // Status 500 Internal Server Error
                }
            }).WithName("CreatePerson")
              .WithSummary("Creates a new person.")
              .WithDescription("Creates a new person with the provided details. Returns 201 if the person is successfully created, or 400 if there is a validation error.")
              .Produces(StatusCodes.Status201Created)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status500InternalServerError);







        }
    }
}
