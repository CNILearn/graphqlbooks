namespace BookSample.ReviewAPI.Parameters;

/// <summary>
/// This class is used to bind the bookIds parameter from the query string. <br />
/// It enables the use of two different formats for the bookIds parameter:
/// <list type="bullet">
///     <item>
///         The first format is a single comma-separated string (bookIds=1,2,3).
///     </item>
///     <item>
///         The second format is multiple query parameters (bookIds=1&amp;bookIds=2&amp;bookIds=3).
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// In order to make use of the parameter binding, the parameter must be added <b>without</b> [AsParameter] to the endpoint method.
/// </remarks>
internal class BookIdsParameter
{
    /// <summary>
    /// The bookIds parameter from the query string.
    /// </summary>
    public long[] BookIds { get; set; } = [];

    public static ValueTask<BookIdsParameter> BindAsync(HttpContext context)
    {
        var parameters = new BookIdsParameter();

        if (!context.Request.Query.TryGetValue("bookIds", out var bookIds) || bookIds.Count == 0)
            return ValueTask.FromResult(parameters);

        if (bookIds.Count == 1)
        {
            // In case the parameters are provided as a single comma-separated string (bookIds=1,2,3)
            parameters.BookIds = bookIds[0]?
                .Split(',')
                .Select(long.Parse)
                .ToArray() ?? [];
        }
        else
        {
            // bookIds.Count > 1
            // In case the parameters are provided as multiple query parameters (bookIds=1&bookIds=2&bookIds=3)
            parameters.BookIds = bookIds
                .Where(x => x is not null)
                .Select(long.Parse!)
                .ToArray();
        }

        return ValueTask.FromResult(parameters);
    }
}