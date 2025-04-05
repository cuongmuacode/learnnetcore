using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnNetCore.Domain;

public class RelationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public Guid CategoryId { get; set; }
    public string? CreatedUserId { get; set; }
}
