using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Mappers;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using Microsoft.AspNetCore.Mvc;


namespace LearnNetCore.Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/articles")]
public class ArticleController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="articleRepository"></param>
    public ArticleController(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ArticleResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] ArticleRequestModel article)
    {
        var articleEntity = article.ToArticleEntity();
        var result = await _articleRepository.SaveAsync(articleEntity, string.Empty);
        var res = result.ToArticleResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="article"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArticleResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ArticleRequestModel article)
    {
        var articleEntity = await _articleRepository.FindAsync(id);
        if (article == null) throw new Exception("Không tìm thấy article");

        var result = await _articleRepository.SaveAsync(articleEntity, string.Empty);
        var res = result.ToArticleResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(PaginationResponse<ArticleResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromBody] ArticleQueryModel queryModel)
    {
        var articles = await _articleRepository.GetAllAsync(queryModel);
        var res = articles?.Items?.Select(x => ArticleMapper.ToArticleResponseModel(x));
        return Ok(
            new PaginationResponse<ArticleResponseModel>()
            {
                Data = new Pagination<ArticleResponseModel>(
                    res,
                    articles.TotalCount,
                    articles.TotalPage,
                    articles.CurrentPage,
                    articles.PageSize),
                Message = "Success",
                StatusCode = 200,
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ArticleResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var article = await _articleRepository.DeleteAsync(id);
        if (article == null) throw new Exception("Không tìm thấy article");
        var res = article.ToArticleResponseModel();
        return Ok(res);
    }
}
