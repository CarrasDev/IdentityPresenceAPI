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




        }

    }
}
