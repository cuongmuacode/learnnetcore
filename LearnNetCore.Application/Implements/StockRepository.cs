using LearnNetCore.Application.Interfaces;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Application.Implements;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private const string CacheKey = "StockList";

    public StockRepository(ApplicationDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<StockEntity?> DeleteAsync(Guid id)
    {
        var stock = await FindAsync(id);
        if (stock == null) return stock;
        _dbContext.Stocks.Remove(stock);
        InvalidCache(stock.Id);
        return stock;
    }

    public async Task<StockEntity?> FindAsync(Guid id)
    {
        var key = BuildCacheKey(id);
        return await _cacheService.GetOrCreateAsync(key, async () =>
        {
            var stock = await FindWithoutCacheAsync(id);
            return stock;
        });
    }
    public async Task<StockEntity?> FindWithoutCacheAsync(Guid id)
    {
        var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        return stock;
    }

    public async Task<Pagination<StockEntity>> GetAllAsync(StockQueryModel queryModel)
    {
        IQueryable<StockEntity> query = _dbContext.Stocks.AsQueryable<StockEntity>();
        if (!string.IsNullOrEmpty(queryModel.CompanyName))
        {
            query = query.Where(x => x.CompanyName.Contains(queryModel.CompanyName));
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

        return new Pagination<StockEntity>(
            items,
            totalCount,
            (int)Math.Ceiling((double)totalCount / queryModel.PageSize),
            queryModel.CurrentPage,
            queryModel.PageSize);

    }

    public async Task<StockEntity> SaveAsync(StockEntity stock)
    {
        var exist = await FindWithoutCacheAsync(stock.Id);
        if (exist == null)
        {
            exist = stock;
            _dbContext.Stocks.Add(stock);
        }
        else
        {
            exist.LastDiv = stock.LastDiv;
            exist.CompanyName = stock.CompanyName;
            exist.Purchase = stock.Purchase;
            exist.Industry = stock.Industry;
            exist.MarketCapitalization = stock.MarketCapitalization;
            exist.Symbol = stock.Symbol;
            _dbContext.Stocks.Update(exist);
        }
        await _dbContext.SaveChangesAsync();

        InvalidCache(stock.Id);

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
