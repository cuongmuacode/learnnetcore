using LearnNetCore.Application.Interfaces;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Application.Implements;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CommentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommentEntity?> DeleteAsync(Guid id)
    {
        var comment = await FindAsync(id);
        if (comment == null) return comment;
        _dbContext.Comments.Remove(comment);
        return comment;
    }

    public async Task<CommentEntity?> FindAsync(Guid id)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        return comment;
    }

    public async Task<Pagination<CommentEntity>> GetAllAsync(CommentQueryModel queryModel)
    {
        IQueryable<CommentEntity> query = _dbContext.Comments.AsQueryable<CommentEntity>();

        if (queryModel.Id != null && queryModel.Id != Guid.Empty)
        {
            query = query.Where(x => x.Id == queryModel.Id.Value);
        }
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Id)
            .Skip(queryModel.CurrentPage)
            .Take(queryModel.PageSize).ToListAsync();

        return new Pagination<CommentEntity>(
            items,
            totalCount,
            (int)Math.Ceiling((double)totalCount / queryModel.PageSize),
            queryModel.CurrentPage,
            queryModel.PageSize);
    }

    public async Task<CommentEntity> SaveAsync(CommentEntity comment)
    {
        var exist = await FindAsync(comment.Id);
        if (exist == null)
        {
            exist = comment;
            _dbContext.Comments.Add(comment);
        }
        else
        {
            exist.Content = comment.Content;
            exist.Title = comment.Title;
            exist.CreatedOnDate = comment.CreatedOnDate;
            exist.StockId = comment.StockId;
            _dbContext.Comments.Update(exist);
        }
        await _dbContext.SaveChangesAsync();
        return exist;
    }
}
