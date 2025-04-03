using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCore.Application.Models;

public class StockResponseModel
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Purchase { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCapitalization { get; set; }
    public List<CommentResponseModel> Comments { get; set; } = new List<CommentResponseModel>();
}
