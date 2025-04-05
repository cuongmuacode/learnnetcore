using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Cache;
using LearnNetCore.Application.Extensions;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.EntityFrameworkCore.Articles;

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private const string CacheKey = "ArticleList";

    public ArticleRepository(ApplicationDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<ArticleEntity?> DeleteAsync(Guid id)
    {
        var article = await FindAsync(id);
        if (article == null) return article;
        _dbContext.Articles.Remove(article);
        InvalidCache(article.Id);
        return article;
    }

    public async Task<ArticleEntity?> FindAsync(Guid id)
    {
        var key = BuildCacheKey(id);
        return await _cacheService.GetOrCreateAsync(key, async () =>
        {
            var stock = await FindWithoutCacheAsync(id);
            return stock;
        });
    }
    public async Task<ArticleEntity?> FindWithoutCacheAsync(Guid id)
    {
        var stock = await _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == id);
        return stock;
    }

    public async Task<Pagination<ArticleEntity>> GetAllAsync(ArticleQueryModel queryModel)
    {
        IQueryable<ArticleEntity> query = _dbContext.Articles.AsQueryable<ArticleEntity>();
        if (!string.IsNullOrEmpty(queryModel.Title))
        {
            query = query.Where(x => x.Title.Contains(queryModel.Title));
        }
        if (queryModel.Id != null && queryModel.Id != Guid.Empty)
        {
            query = query.Where(x => x.Id == queryModel.Id.Value);
        }
        if (!string.IsNullOrWhiteSpace(queryModel.FullTextSearch))
        {
            var ts = queryModel.FullTextSearch;
            query = query.Where(q =>
                q.Content!.Contains(ts) || q.Title.Contains(ts));
        }
        queryModel.Sort ??= "Id";
        query.OrderBy(x => x.Id);
        return await query.Page(queryModel.CurrentPage, queryModel.PageSize, queryModel.Sort);
    }

    public async Task<ArticleEntity> SaveAsync(ArticleEntity article, string userId)
    {
        var exist = await FindWithoutCacheAsync(article.Id);
        if (exist == null)
        {
            article.CreatedOnDate = DateTime.Now;
            article.CreatedUserId = userId;
            article.LastModifiedOnDate = DateTime.Now;
            article.LastModifiedUserId = userId;
            exist = article;
            _dbContext.Articles.Add(article);
        }
        else
        {
            exist.Content = article.Content;
            exist.Title = article.Title;
            exist.CreatedOnDate = article.CreatedOnDate;
            exist.LastModifiedOnDate = DateTime.Now;
            exist.CreatedUserId = article.CreatedUserId;
            exist.LastModifiedUserId = userId;
            _dbContext.Articles.Update(exist);
        }
        await _dbContext.SaveChangesAsync();

        InvalidCache(article.Id);

        return exist;
    }
    private string BuildCacheKey(Guid id)
    {
        return CacheKey + id;
    }
    private void InvalidCache(Guid id)
    {
        var key = BuildCacheKey(id);
        _cacheService.Remove(key);
    }
}
