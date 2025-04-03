using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Mappers;

public static class StockMapper
{
    public static StockEntity ToStockEntity(this StockRequestModel model)
    {
        return new StockEntity
        {
            Id = model.Id,
            CompanyName = model.CompanyName,
            Industry = model.Industry,
            LastDiv = model.LastDiv,
            MarketCapitalization = model.MarketCapitalization,
            Purchase = model.Purchase,
            Symbol = model.Symbol
        };
    }
    public static StockResponseModel ToStockResponseModel(this StockEntity entity)
    {
        return new StockResponseModel
        {
            Id = entity.Id,
            CompanyName = entity.CompanyName,
            Industry = entity.Industry,
            LastDiv = entity.LastDiv,
            MarketCapitalization = entity.MarketCapitalization,
            Purchase = entity.Purchase,
            Symbol = entity.Symbol,
        };
    }

    public static StockRequestModel ToStockRequestModel(this StockEntity entity)
    {
        return new StockRequestModel
        {
            Id = entity.Id,
            CompanyName = entity.CompanyName,
            Industry = entity.Industry,
            LastDiv = entity.LastDiv,
            MarketCapitalization = entity.MarketCapitalization,
            Purchase = entity.Purchase,
            Symbol = entity.Symbol
        };
    }
}
