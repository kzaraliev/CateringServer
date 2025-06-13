using Catering.Core.DTOs.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catering.Core.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, bool descending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string method = descending ? "OrderByDescending" : "OrderBy";
            var expression = Expression.Call(typeof(Queryable), method, new[] { typeof(T), property.Type },
                query.Expression, Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(expression);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static async Task<PagedResult<TResult>> ToPagedResultAsync<T, TResult>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            Expression<Func<T, TResult>> selector)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
                Items = items
            };
        }
    }

}
