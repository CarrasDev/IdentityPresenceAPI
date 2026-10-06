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
            })
            .WithName("GetPersonById")
            .WithSummary("Gets a person by their unique identifier.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        }
    }
}
