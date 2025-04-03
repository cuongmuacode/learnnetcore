using LearnNetCore.Application;
using LearnNetCore.Application.Interfaces;
using LearnNetCore.Application.Mappers;
using LearnNetCore.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/comments")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="commentRepository"></param>
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="comment"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CommentResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CommentRequestModel comment)
    {
        var commentEntity = comment.ToCommentEntity();
        var result = await _commentRepository.SaveAsync(commentEntity);
        var res = result.ToCommentResponseModel();
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
    [ProducesResponseType(typeof(CommentResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CommentRequestModel comment)
    {
        var commentEntity = await _commentRepository.FindAsync(id);
        if (comment == null) throw new Exception("Không tìm thấy comment");

        var result = await _commentRepository.SaveAsync(commentEntity!);
        var res = result.ToCommentResponseModel();
        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(Pagination<CommentResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromBody] CommentQueryModel queryModel)
    {
        var comments = await _commentRepository.GetAllAsync(queryModel);
        var res = comments.Items.Select(x => x.ToCommentResponseModel());
        return Ok(
            new Pagination<CommentResponseModel>(
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
    [ProducesResponseType(typeof(CommentResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var comment = await _commentRepository.DeleteAsync(id);
        if (comment == null) throw new Exception("Không tìm thấy comment");
        var res = comment.ToCommentResponseModel();
        return Ok(res);
    }
}
