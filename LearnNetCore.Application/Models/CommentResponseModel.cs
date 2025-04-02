using LearnNetCore.Domain;

namespace LearnNetCore.Application.Models;

public class CommentResponseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOnDate { get; set; } = DateTime.Now;
    public Guid? StockId { get; set; }
    public StockEntity Stock { get; set; }

}
