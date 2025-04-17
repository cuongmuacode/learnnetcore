using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCore.Application.Models;

public class ArticleRequestModel
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedOnDate { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }
}
