namespace Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public static class CursorPagination
{
    /// cursor-based pagination with support for composite cursor => on two column if needed (require index for the compostie columns)
    /// query should passed after apply "orderby" based on column
    public static async Task<CursorPage<T>> ToPagedResultAsync<T>(IQueryable<T> query, int limit) where T : class
    {
        var items = await query.Take(limit + 1).ToListAsync();

        bool hasMore = items.Count > limit;

        if (hasMore)
            items.RemoveAt(limit);

        return new CursorPage<T>
        {
            Items = items,
            HasMore = hasMore
        };
    }

    public static async Task<CursorPagedResult<T>> ToCursorPageAsync<T,TValue,TKey>(this IQueryable<T> query,int limit,Func<T, (TValue, TKey)> getCursorProps) where T : class
    {
        var items = await query.Take(limit + 1).ToListAsync();

        bool hasMore = items.Count > limit;

        if (hasMore)
        {
            items.RemoveAt(limit); 
        }

        string? nextCursor = null;
        if (items.Any())
        {
            var lastItem = items.Last();
            var (date,id) = getCursorProps(lastItem);
            nextCursor = CursorHelper.Encode(date, id); 
        }

        return new CursorPagedResult<T>
        {
            Items = items,
            HasMore = hasMore,
            NextCursor = nextCursor
        };
    }

}