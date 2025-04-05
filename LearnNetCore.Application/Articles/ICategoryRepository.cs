using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Articles;

public interface ICategoryRepository
{
    Task<Pagination<CategoryEntity>> GetAllAsync(CategoryQueryModel queryModel);
    Task<CategoryEntity?> FindAsync(Guid id);
    Task<CategoryEntity> SaveAsync(CategoryEntity category, string userId);
    Task<CategoryEntity?> DeleteAsync(Guid id);
}
