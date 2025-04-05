using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Application;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CategoryEntity?> DeleteAsync(Guid id)
    {
        var comment = await FindAsync(id);
        if (comment == null) return comment;
        _dbContext.Categories.Remove(comment);
        return comment;
    }

    public async Task<CategoryEntity?> FindAsync(Guid id)
    {
        var comment = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        return comment;
    }

    public async Task<Pagination<CategoryEntity>> GetAllAsync(CategoryQueryModel queryModel)
    {
        IQueryable<CategoryEntity> query = _dbContext.Categories.AsQueryable<CategoryEntity>();

        if (queryModel.Id != null && queryModel.Id != Guid.Empty)
        {
            query = query.Where(x => x.Id == queryModel.Id.Value);
        }
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Id)
            .Skip(queryModel.CurrentPage)
            .Take(queryModel.PageSize).ToListAsync();

        return new Pagination<CategoryEntity>(
            items,
            totalCount,
            (int)Math.Ceiling((double)totalCount / queryModel.PageSize),
            queryModel.CurrentPage,
            queryModel.PageSize);
    }

    public async Task<CategoryEntity> SaveAsync(CategoryEntity category, string userId)
    {
        var exist = await FindAsync(category.Id);
        if (exist == null)
        {
            category.UserId = userId;

            exist = category;
            _dbContext.Categories.Add(category);
        }
        else
        {
            exist.Content = category.Content;
            exist.Description = category.Description;
            exist.Name = category.Name;
            exist.UserId = userId;

            _dbContext.Categories.Update(exist);
        }
        await _dbContext.SaveChangesAsync();
        return exist;
    }
}
