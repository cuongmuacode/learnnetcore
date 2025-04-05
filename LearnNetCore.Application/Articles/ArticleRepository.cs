using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Cache;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Application;

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
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Id)
            .Skip(queryModel.CurrentPage)
            .Take(queryModel.PageSize).ToListAsync();

        return new Pagination<ArticleEntity>(
            items,
            totalCount,
            (int)Math.Ceiling((double)totalCount / queryModel.PageSize),
            queryModel.CurrentPage,
            queryModel.PageSize);

    }

    public async Task<ArticleEntity> SaveAsync(ArticleEntity article, string userId)
    {
        var exist = await FindWithoutCacheAsync(article.Id);
        if (exist == null)
        {
            article.UserId = userId;
            exist = article;
            _dbContext.Articles.Add(article);
        }
        else
        {
            exist.CreatedOnDate = article.CreatedOnDate;
            exist.Content = article.Content;
            exist.Title = article.Title;
            exist.LastModifiedOnDate = article.LastModifiedOnDate;
            exist.UserId = userId;

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
