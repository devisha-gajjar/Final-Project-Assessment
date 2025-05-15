namespace WebApp.Service.Helper;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; } = 1;
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; } = 5;
    public int TotalItems { get; private set; }
    public string SearchTerm { get; set; }
    public string SortOrder { get; set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalItems = count;

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
