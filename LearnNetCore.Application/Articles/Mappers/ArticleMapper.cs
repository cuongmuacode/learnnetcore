using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Mappers;

public static class ArticleMapper
{
    public static ArticleEntity ToArticleEntity(this ArticleRequestModel model)
    {
        return new ArticleEntity
        {
            Id = model.Id,
            Content = model.Content,
            CreatedOnDate = model.CreatedOnDate,
            LastModifiedOnDate = model.LastModifiedOnDate,
            Title = model.Title

        };
    }
    public static ArticleResponseModel ToArticleResponseModel(this ArticleEntity entity)
    {
        return new ArticleResponseModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            CreatedOnDate = entity.CreatedOnDate,
            LastModifiedOnDate = entity.LastModifiedOnDate,
        };
    }

    public static ArticleRequestModel ToArticleRequestModel(this ArticleEntity entity)
    {
        return new ArticleRequestModel
        {
            Title = entity.Title,
            Content = entity.Content,
            LastModifiedOnDate = entity.LastModifiedOnDate,
            CreatedOnDate = entity.CreatedOnDate,
            Id = entity.Id
        };
    }
}
