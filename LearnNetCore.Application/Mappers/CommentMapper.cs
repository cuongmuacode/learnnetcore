using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Mappers;

public static class CommentMapper
{
    public static CommentEntity ToCommentEntity(this CommentRequestModel model)
    {
        return new CommentEntity
        {
            Id = model.Id,
            Content = model.Content,
            StockId = model.StockId,
            CreatedOnDate = model.CreatedOnDate,
            Title = model.Title
        };
    }
    public static CommentResponseModel ToCommentResponseModel(this CommentEntity entity)
    {
        return new CommentResponseModel
        {
            Id = entity.Id,
            Content = entity.Content,
            StockId = entity.StockId,
            CreatedOnDate = entity.CreatedOnDate,
            Title = entity.Title,
        };
    }

    public static CommentRequestModel ToCommentRequestModel(this CommentEntity entity)
    {
        return new CommentRequestModel
        {
            Id = entity.Id,
            Content = entity.Content,
            StockId = entity.StockId,
            CreatedOnDate = entity.CreatedOnDate,
            Title = entity.Title,
        };
    }
}
