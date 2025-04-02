using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Interfaces;

public interface IStockRepository
{
    Task<IEnumerable<StockEntity>> GetAllAsync(StockQueryModel queryModel);
    Task<StockEntity?> FindAsync(Guid id);
    Task<StockEntity> SaveAsync(StockEntity stock);
    Task<StockEntity?> DeleteAsync(Guid id);

}
