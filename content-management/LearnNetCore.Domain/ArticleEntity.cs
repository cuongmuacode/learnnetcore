using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCore.Domain;

[Table("Articles")]
public class ArticleEntity : IIdEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? CreatedOnDate { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }
    public string? CreatedUserId { get; set; }
    public string? LastModifiedUserId { get; set; }
}
