using LearnNetCore.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Web.Models;

public class ArticleViewModel
{
    public List<ArticleResponseModel> Articles { get; set; } = new List<ArticleResponseModel>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
