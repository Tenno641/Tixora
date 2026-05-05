using ErrorOr;

namespace Events.Domain.Categories;

public static class CategoryErrors
{
    public static Error CategoryNotFound = Error.NotFound(code: "Get.Category", description: "Category not found");
    public static Error CategoryAlreadyArchived = Error.Conflict(code: "Category.Archive", description: "Category is already archived");
}