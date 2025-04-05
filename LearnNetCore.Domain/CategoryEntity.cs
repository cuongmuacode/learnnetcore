using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCore.Domain;

[Table("Categories")]
public class CategoryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? CreatedOnDate { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }
    public string? CreatedUserId { get; set; }
    public string? LastModifiedUserId { get; set; }

}