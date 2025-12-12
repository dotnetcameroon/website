using System.Dynamic;
using ErrorOr;

namespace app.Utilities;

public static class ProblemUtilities
{
    public static IResult Problem(List<Error>? errors = null)
    {
        if (errors is null || errors.Count == 0)
            return Results.Problem();

        Error firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var extensions = new Dictionary<string, object?>()
        {
            { "errors", errors.Select(e => e.Description) }
        };

        if (firstError.Metadata is { Count: > 0 })
        {
            extensions.Add("metadata", errors
                .Select(e => e.Metadata)
                .ToObject());
        }

        return Results.Problem(
            title: firstError.Description,
            statusCode: statusCode,
            extensions: extensions);
    }

    private static dynamic ToObject(this IEnumerable<Dictionary<string, object>?> dictionaries)
    {
        dynamic obj = new ExpandoObject();
        var objDictionary = (IDictionary<string, object>)obj;
        foreach (var dictionary in dictionaries)
        {
            if (dictionary is not { Count: > 0 })
                continue;

            foreach (var kvp in dictionary)
            {
                objDictionary.Add(kvp.Key, kvp.Value);
            }
        }

        return obj;
    }

    public static void Problem(HttpContext context, Error[]? errors = null)
    {
        var result = Results.Problem();
        if (errors is null || errors.Length == 0)
        {
            result.ExecuteAsync(context);
            return;
        }

        Error firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var extensions = new Dictionary<string, object?>()
        {
            { "errors", errors.Select(e => e.Description) }
        };

        if (firstError.Metadata is { Count: > 0 })
        {
            extensions.Add("metadata", errors
                .Select(e => e.Metadata)
                .ToObject());
        }

        result = Results.Problem(
            title: firstError.Description,
            statusCode: statusCode,
            extensions: extensions);

        result.ExecuteAsync(context);
    }
}