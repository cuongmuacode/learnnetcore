
namespace LearnNetCore.Domain;

public class Pagination<T>
{
    public Pagination(int totalCount, int currentPage, int pageSize)
    {
        TotalCount = totalCount;
        TotalPage = (int)Math.Ceiling((double)totalCount / pageSize);
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
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
    public int TotalCount { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T>? Items { get; set; }
}

public class PaginationResponse<T> : Response
{
    public Pagination<T> Data { get; set; }
}
