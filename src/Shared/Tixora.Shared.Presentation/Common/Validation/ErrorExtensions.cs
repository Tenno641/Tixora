using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Tixora.Shared.Presentation.Common.Validation;

public static class ErrorExtensions
{
    public static IResult ToProblemDetails(this IErrorOr errorOr)
    {
        List<Error>? errors = errorOr.Errors;

        if (errors is null || errors.Count == 0)
            return Results.Ok();

        if (errors.All(e => e.Type == ErrorType.Validation))
            return Results.ValidationProblem(ValidationFailure(errors));

        Error error = errors.First();

        return Results.Problem(
            statusCode: ErrorTypeToStatusCode(error.Type),
            type: error.Code,
            detail: error.Description, 
            extensions: new Dictionary<string, object?>()
            {
                ["errors"] = errors.Select(e => new {e.Code, e.Description})
            });
    }

    private static Dictionary<string, string[]> ValidationFailure(List<Error> errors)
    {
        Dictionary<string, string[]> validationFailures = errors.GroupBy(e => e.Code)
            .ToDictionary(group => group.Key, group => group.Select(g => g.Description).ToArray());

        return validationFailures;
    }

    private static int ErrorTypeToStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.Failure => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };
}