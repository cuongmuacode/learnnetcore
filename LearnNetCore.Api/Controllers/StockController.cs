using LearnNetCore.Application.Interfaces;
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
    private readonly IStockRepository _stockRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stockRepository"></param>
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stock"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(StockResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] StockRequestModel stock)
    {
        var stockEntity = stock.ToStockEntity();
        var result = await _stockRepository.SaveAsync(stockEntity);
        var res = result.ToStockResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="stock"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(StockResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] StockRequestModel stock)
    {
        var stockEntity = await _stockRepository.FindAsync(id);
        if (stock == null) throw new Exception("Không tìm thấy stock");

        var result = await _stockRepository.SaveAsync(stockEntity);
        var res = result.ToStockResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(List<StockResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromBody] StockQueryModel queryModel)
    {
        var stocks = await _stockRepository.GetAllAsync(queryModel);
        var res = stocks.Select(x => StockMapper.ToStockResponseModel(x));
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(StockResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var stock = await _stockRepository.DeleteAsync(id);
        if (stock == null) throw new Exception("Không tìm thấy stock");
        var res = stock.ToStockResponseModel();
        return Ok(res);
    }

}
