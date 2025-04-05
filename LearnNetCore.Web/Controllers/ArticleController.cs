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

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id != null)
            {
                var article = await _articleService.FindAsync(id.Value);
                return View(article?.ToArticleRequestModel() ?? new ArticleRequestModel());
            }

            return View(new ArticleRequestModel());
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOrCreate(
             ArticleRequestModel article)
        {
            await _articleService.SaveAsync(article.ToArticleEntity(), string.Empty);
            return RedirectToAction("Index", "Article");
        }

        public async Task<IActionResult> Index(ArticleQueryModel queryModel)
        {
            const int pageSize = 10;
                var result = await _articleService.GetAllAsync(queryModel);
            var model = new ArticleViewModel
            {
                Articles = result?.Items?.Select(x =>
                    x.ToArticleResponseModel())?.ToList(),
                CurrentPage = queryModel.CurrentPage,
                TotalPages = result.TotalPage
            };

            return View(model);
        }
    }
}
