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
                    return Results.InternalServerError(new { error = ex.Message }); // Status 500 Internal Server Error
                }
            }).WithName("CreatePerson")
              .WithSummary("Creates a new person.")
              .WithDescription("Creates a new person with the provided details. Returns 201 if the person is successfully created,or 400 if there is a validation error.")
              .Produces(StatusCodes.Status201Created)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/", async (GetAllPersonsUseCase useCase) =>
            {
                try
                {
                    var persons = await useCase.ExecuteAsync();
                    return Results.Ok(persons);                                     // Status 200 OK
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message }); // Status 500 Internal Server Error
                }
            }).WithName("GetAllPersons")
              .WithSummary("Gets all persons.")
              .WithDescription("Retrieves a list of all persons. Returns 200 if the list is successfully retrieved.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapPut("/{id:guid}", async (Guid id, UpdatePersonDto dto, UpdatePersonUseCase useCase) =>
            {
                if (id != dto.Id)
                {
                    return Results.BadRequest("ID in the URL does not match ID in the request body."); // Status 400 Bad Request
                }

                try
                {
                    var person = await useCase.ExecuteAsync(dto);
                    return Results.Ok(person);                                       // Status 200 OK
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });             // Status 404 Not Found
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });           // Status 400 Bad Request
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("UpdatePerson")
              .WithSummary("Updates an existing person.")
              .WithDescription("Updates the details of an existing person. Returns 200 if the update is successful, 400 if there is a validation error, or 404 if the person is not found.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapDelete("/{id:guid}", async (Guid id, DeletePersonUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();                                      // Status 204 No Content
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });             // Status 404 Not Found
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("DeletePerson")
              .WithSummary("Deletes an existing person.")
              .WithDescription("Deletes an existing person. Returns 204 if the deletion is successful, or 404 if the person is not found.")
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/code/{code}", async (string code, GetPersonByCodeUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(code);
                    return Results.Ok(person);                                       // Status 200 OK
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });             // Status 404 Not Found
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("GetPersonByCode")
              .WithSummary("Gets a person by code.")
              .WithDescription("Gets a person by their unique code. Returns 200 if the person is found, or 404 if the person is not found.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status500InternalServerError);


        }
    }
}
