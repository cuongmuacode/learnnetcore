namespace LearnNetCore.Domain;

public class Response
{
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}


public class ResponseObject<T> : Response
{
    public T Data { get; set; }
}