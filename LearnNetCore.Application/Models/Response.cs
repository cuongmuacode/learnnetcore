namespace LearnNetCore.Application.Models;

public class Response<T>
{
    public T? Message { get; set; }
    public string? Detail { get; set; }
    public int StatusCode { get; set; }
}
