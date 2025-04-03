namespace LearnNetCore.Application;

public class Pagination<T>
{
    public Pagination(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        TotalPage = (int)Math.Ceiling((double)totalCount / pageSize);
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    public Pagination(IEnumerable<T> items, int totalCount, int totalPage, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        TotalPage = totalPage;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    public int TotalCount { get; }
    public int TotalPage { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public IEnumerable<T> Items { get; }
}

