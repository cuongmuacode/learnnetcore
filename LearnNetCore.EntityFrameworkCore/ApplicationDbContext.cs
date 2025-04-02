using LearnNetCore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnNetCore.EntityFrameworkCore;

public class ApplicationDbContext : DbContext   
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<StockEntity> Stocks { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

}
