using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Articles;

public interface IArticleRepository
{
    Task<Pagination<ArticleEntity>> GetAllAsync(ArticleQueryModel queryModel);
    Task<ArticleEntity?> FindAsync(Guid id);
    Task<ArticleEntity> SaveAsync(ArticleEntity article, string userId);
    Task<ArticleEntity?> DeleteAsync(Guid id);
}
