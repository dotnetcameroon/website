namespace app.Utilities;
public class PagedList<T> : PagedList
{
    public List<T> Items { get; private set; } = [];
    internal PagedList(List<T> items)
    {
        Items = items;
    }
}

public class PagedList
{
    public int TotalCount { get; private set; }
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static PagedList<T> Empty<T>()
    {
        return new PagedList<T>([])
        {
            TotalCount = 0,
            PageNumber = 0,
            PageSize = 0
        };
    }

    public static PagedList<T> FromList<T>(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        return new PagedList<T>(items)
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
