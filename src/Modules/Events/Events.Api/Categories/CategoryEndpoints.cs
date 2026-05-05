using Microsoft.AspNetCore.Routing;

namespace Events.Api.Categories;

public static class CategoryEndpoints
{
    public static void AddEndpoints(IEndpointRouteBuilder app)
    {
        ArchiveCategory.AddEndpoint(app);
        CreateCategory.AddEndpoint(app);
        GetCategory.AddEndpoint(app);
        GetCategories.AddEndpoint(app);
        UpdateCategory.AddEndpoint(app);
    }
}
