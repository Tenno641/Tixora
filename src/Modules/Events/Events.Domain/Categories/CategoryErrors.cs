using ErrorOr;

namespace Events.Domain.Categories;

public static class CategoryErrors
{
    public static Error CategoryNotFound = Error.NotFound(code: "Get.Category", description: "Category not found");
}