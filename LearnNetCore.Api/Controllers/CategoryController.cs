using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Mappers;
using LearnNetCore.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryRepository"></param>
    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryRequestModel category)
    {
        var categoryEntity = category.ToCategoryEntity();
        var result = await _categoryRepository.SaveAsync(categoryEntity, string.Empty);
        var res = result.ToCategoryResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CategoryRequestModel comment)
    {
        var commentEntity = await _categoryRepository.FindAsync(id);
        if (comment == null) throw new Exception("Không tìm thấy category");

        var result = await _categoryRepository.SaveAsync(commentEntity!, string.Empty);
        var res = result.ToCategoryResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(Pagination<CategoryResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromBody] CategoryQueryModel queryModel)
    {
        var comments = await _categoryRepository.GetAllAsync(queryModel);
        var res = comments.Items.Select(x => x.ToCategoryResponseModel());
        return Ok(
            new Pagination<CategoryResponseModel>(
                res,
                comments.TotalCount,
                comments.TotalPage,
                comments.CurrentPage,
                comments.PageSize)
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var comment = await _categoryRepository.DeleteAsync(id);
        if (comment == null) throw new Exception("Không tìm thấy category");
        var res = comment.ToCategoryResponseModel();
        return Ok(res);
    }
}
