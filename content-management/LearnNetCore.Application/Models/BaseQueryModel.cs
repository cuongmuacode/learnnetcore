namespace LearnNetCore.Application;

public class BaseQueryModel
{
    public Guid? Id { get; set; }
    public int PageSize { get; set; } = 20;
    public int CurrentPage { get; set; } = 1;
    public string? FullTextSearch { get; set; }
    public IEnumerable<string>? ListTextSearch { get; set; }
    public string? Sort { get; set; }
    public string? Name { get; set; }
}