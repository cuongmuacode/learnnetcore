using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNetCore.Application.Extensions;

public static class IQueryableExtensions
{
    public static async Task<Pagination<T>> Page<T>(this IQueryable<T> query, int currentPage, int pageSize, string sortExpression)
    {
        Pagination<T> result = new Pagination<T>(await query.CountAsync(), currentPage, pageSize);
        int count = (currentPage - 1) * pageSize;
        Pagination<T> pagination = result;
        query.Skip(count).Take(pageSize);
        pagination.Items = await query.ToListAsync();
        return result;

    }

}
