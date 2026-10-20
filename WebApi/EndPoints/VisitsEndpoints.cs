using Application.DTOs.Visits;
using Application.UseCases.Visits;

namespace WebApi.EndPoints
{
    public static class VisitsEndpoints
    {
        public static void MapVisitsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/visits")
                           .WithTags("Visits");

            group.MapGet("/", async (GetAllVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);                                       // Status 200 OK
                }
                catch(Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("GetAllVisits")
              .WithSummary("Get all visits")
              .WithDescription("Retrieves a list of all visits from the system.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapPost("/entry", async (RegisterEntryDto dto, RegisterEntryUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/visits/{visit.Id}", visit);       // Status 201 Created
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
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("RegisterEntry")
              .WithSummary("Register a new visit entry")
              .WithDescription("Registers a new visit entry in the system (PersonId or Code).")
              .Produces(StatusCodes.Status201Created)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/active", async (GetActiveVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);                                       // Status 200 OK
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("GetActiveVisits")
              .WithSummary("Get all active visits")
              .WithDescription("Retrieves a list of all active visits from the system and persons associated with them.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/person/{personId:guid}", async (Guid personId, GetVisitsByPersonUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync(personId);
                    return Results.Ok(visits);                                       // Status 200 OK
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("GetVisitsByPerson")
              .WithSummary("Get visits by person")
              .WithDescription("Retrieves a list of all visits associated with a specific person.")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapPost("/exit", async (RegisterExitDto dto, RegisterExitUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Ok(visit);                                        // Status 200 OK
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
                    return Results.InternalServerError(new { error = ex.Message });  // Status 500 Internal Server Error
                }
            }).WithName("RegisterExit")
              .WithSummary("Register a visit exit")
              .WithDescription("Registers the exit of a visit in the system (VisitId or Code).")
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status500InternalServerError);

        }

    }
}
