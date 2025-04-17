using LearnNetCore.Domain;
using System.ComponentModel.DataAnnotations;

namespace LearnNetCore.Application.Models;

public class CategoryResponseModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Description { get; set; }
}
