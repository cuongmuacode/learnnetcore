using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Mappers;
using LearnNetCore.Application.Models;
using LearnNetCore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleService;

        public ArticleController(IArticleRepository articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            const int pageSize = 10;
            var result = await _articleService.GetAllAsync(new ArticleQueryModel
            {
                CurrentPage = currentPage,
                PageSize = pageSize
            });
            var model = new ArticleViewModel
            {
                Articles = result.Items.Select(x =>
                    x.ToArticleResponseModel()).ToList(),
                CurrentPage = currentPage,
                TotalPages = result.TotalPage
            };

            return View(model);
        }
    }
}
