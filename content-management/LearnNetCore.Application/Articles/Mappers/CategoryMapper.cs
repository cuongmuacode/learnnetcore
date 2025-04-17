using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryEntity ToCategoryEntity(this CategoryRequestModel model)
    {
        return new CategoryEntity
        {
            Id = model.Id,
            Content = model.Content,
            Code = model.Code,
            Description = model.Description,
            Name = model.Name
        };
    }
    public static CategoryResponseModel ToCategoryResponseModel(this CategoryEntity entity)
    {
        return new CategoryResponseModel
        {
            Id = entity.Id,
            Content = entity.Content,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static CategoryRequestModel ToCategoryRequestModel(this CategoryEntity entity)
    {
        return new CategoryRequestModel
        {
            Id = entity.Id,
            Content = entity.Content,
            Description = entity.Description,
            Name = entity.Name,
            Code = entity.Code
        };
    }
}
