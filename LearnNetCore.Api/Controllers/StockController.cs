using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Mappers;
using LearnNetCore.Application.Models;
using Microsoft.AspNetCore.Mvc;


namespace LearnNetCore.Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/stocks")]
public class StockController : ControllerBase
{
    private readonly IArticleRepository _stockRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stockRepository"></param>
    public StockController(IArticleRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stock"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ArticleResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] ArticleRequestModel stock)
    {
        var stockEntity = stock.ToArticleEntity();
        var result = await _stockRepository.SaveAsync(stockEntity);
        var res = result.ToArticleResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="stock"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArticleResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ArticleRequestModel stock)
    {
        var stockEntity = await _stockRepository.FindAsync(id);
        if (stock == null) throw new Exception("Không tìm thấy stock");

        var result = await _stockRepository.SaveAsync(stockEntity);
        var res = result.ToArticleResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(Pagination<ArticleResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromBody] ArticleQueryModel queryModel)
    {
        var stocks = await _stockRepository.GetAllAsync(queryModel);
        var res = stocks.Items.Select(x => ArticleMapper.ToArticleResponseModel(x));
        return Ok(
            new Pagination<ArticleResponseModel>(
                res,
                stocks.TotalCount,
                stocks.TotalPage,
                stocks.CurrentPage,
                stocks.PageSize)
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
        var stock = await _stockRepository.DeleteAsync(id);
        if (stock == null) throw new Exception("Không tìm thấy stock");
        var res = stock.ToArticleResponseModel();
        return Ok(res);
    }
}
