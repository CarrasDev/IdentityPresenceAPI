namespace WebApi.EndPoints
{
    public static class VisitsEndpoints
    {
        public static void MapVisitsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/visits")
                           .WithTags("Visits");







        }

    }
}
