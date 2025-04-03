using LearnNetCore.Application.Models;
using LearnNetCore.Domain;

namespace LearnNetCore.Application.Interfaces;

public interface ICommentRepository
{
    Task<Pagination<CommentEntity>> GetAllAsync(CommentQueryModel queryModel);
    Task<CommentEntity?> FindAsync(Guid id);
    Task<CommentEntity> SaveAsync(CommentEntity stock);
    Task<CommentEntity?> DeleteAsync(Guid id);
}
