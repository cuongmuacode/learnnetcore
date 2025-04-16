using LearnNetCore.Application.Extensions;
using LearnNetCore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public enum ExpressionOption
{
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual
}

public static class QueryableExtensions
{
    /// <summary>
    /// Hỗ trợ sắp xếp theo tên thuộc tính động.
    /// </summary>
    private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool isDescending, bool isThenBy)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = (isThenBy ? "ThenBy" : "OrderBy") + (isDescending ? "Descending" : "");
        var methodCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.Type },
            source.Expression,
            Expression.Quote(lambda)
        );

        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(methodCall);
    }

    // Sắp xếp tăng dần theo thuộc tính
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => OrderingHelper(source, propertyName, isDescending: false, isThenBy: false);

    // Sắp xếp giảm dần theo thuộc tính
    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => OrderingHelper(source, propertyName, isDescending: true, isThenBy: false);

    // Sắp xếp tăng dần tiếp theo
    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        => OrderingHelper(source, propertyName, isDescending: false, isThenBy: true);

    // Sắp xếp giảm dần tiếp theo
    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        => OrderingHelper(source, propertyName, isDescending: true, isThenBy: true);

    /// <summary>
    /// Sắp xếp theo biểu thức chuỗi, ví dụ: "+Name,-Age"
    /// </summary>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string sortExpression)
    {
        var query = source;
        var orderedQuery = query as IOrderedQueryable<T>;
        bool hasSorted = false;

        var sortParts = sortExpression.Split(',', ';').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s));

        int index = 0;
        foreach (var part in sortParts)
        {
            char prefix = part[0];
            string propertyName = (prefix == '+' || prefix == '-') ? part.Substring(1) : part;

            if (!typeof(T).HasProperty(propertyName))
                continue;

            if (index == 0)
            {
                orderedQuery = prefix == '-' ? query.OrderByDescending(propertyName) : query.OrderBy(propertyName);
            }
            else
            {
                orderedQuery = prefix == '-' ? orderedQuery.ThenByDescending(propertyName) : orderedQuery.ThenBy(propertyName);
            }

            hasSorted = true;
            index++;
        }

        return hasSorted ? orderedQuery : query;
    }

    /// <summary>
    /// Tạo biểu thức điều kiện Where với toán tử so sánh cơ bản.
    /// </summary>
    public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, object value, ExpressionOption comparison)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var left = Expression.Property(parameter, propertyName);
        var right = Expression.Constant(value);

        var binaryExpression = comparison switch
        {
            ExpressionOption.Equal => Expression.Equal(left, right),
            ExpressionOption.NotEqual => Expression.NotEqual(left, right),
            ExpressionOption.GreaterThan => Expression.GreaterThan(left, right),
            ExpressionOption.LessThan => Expression.LessThan(left, right),
            ExpressionOption.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            ExpressionOption.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            _ => Expression.Equal(left, right)
        };

        var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
        return source.Where(lambda);
    }

    /// <summary>
    /// Tìm kiếm theo danh sách chứa các giá trị (Contains).
    /// </summary>
    public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string propertyName, IList<Guid> values)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var property = Expression.Property(parameter, propertyName);
        var valueList = Expression.Constant(values);

        var containsMethod = typeof(ICollection<Guid>).GetMethod("Contains", new[] { typeof(Guid) })
                             ?? throw new InvalidOperationException("Contains method not found.");

        var call = Expression.Call(valueList, containsMethod, property);
        var lambda = Expression.Lambda<Func<T, bool>>(call, parameter);

        return source.Where(lambda);
    }
    public static async Task<Pagination<T>> GetPagedAsync<T>(this IQueryable<T> query, int currentPage, int pageSize) where T : class
       => await query.GetPagedOrderAsync(currentPage, pageSize, string.Empty);

    public static async Task<Pagination<T>> GetPagedAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, string sortExpression) where T : class
        => await query.GetPagedOrderAsync(currentPage, pageSize, sortExpression);

    public static async Task<Pagination<T>> GetPagedOrderAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, string sortExpression) where T : class
    {
        if (currentPage < 1) currentPage = 1;
        if (pageSize < 1) pageSize = 10;

        if (!string.IsNullOrWhiteSpace(sortExpression))
        {
            query = query.ApplySorting(sortExpression);
        }

        var totalCount = await query.CountAsync();
        var result = new Pagination<T>(totalCount, currentPage, pageSize);
        result.Items = await query.Paginate(currentPage, pageSize).ToListAsync();
        return result;
    }

    public static async Task<Pagination<Guid>> GetIdsPagedAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, string sortExpression) where T : IIdEntity
    {
        if (currentPage < 1) currentPage = 1;
        if (pageSize < 1) pageSize = 10;

        if (!string.IsNullOrWhiteSpace(sortExpression))
        {
            query = query.ApplySorting(sortExpression);
        }

        var totalCount = await query.CountAsync();
        var result = new Pagination<Guid>(totalCount, currentPage, pageSize);
        result.Items = await query.Paginate(currentPage, pageSize).Select(x => x.Id).ToListAsync();
        return result;
    }

    private static IQueryable<T> Paginate<T>(this IQueryable<T> query, int currentPage, int pageSize)
        => query.Skip((currentPage - 1) * pageSize).Take(pageSize);
}
