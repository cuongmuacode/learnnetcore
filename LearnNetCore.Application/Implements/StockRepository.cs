using LearnNetCore.Application.Interfaces;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Application.Implements;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StockRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StockEntity?> DeleteAsync(Guid id)
    {
        var stock = await FindAsync(id);
        if (stock == null) return stock;
        _dbContext.Stocks.Remove(stock);
        return stock;
    }

    public async Task<StockEntity?> FindAsync(Guid id)
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
        var exist = await FindAsync(stock.Id);
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
        return exist;
    }
}
