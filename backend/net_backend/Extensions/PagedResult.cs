using Microsoft.EntityFrameworkCore;
using CustomAttributes;

namespace CustomExtensions
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }

    public static partial class QueryableExtensions
    {
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> query, PaginationParams pagination)
        {
            var total = await query.CountAsync();
            var items = await query.Skip(pagination.EffectiveOffset).Take(pagination.EffectiveLimit).ToListAsync();

            return new PagedResult<T>
            {
                TotalCount = total,
                Offset = pagination.EffectiveOffset,
                Limit = pagination.EffectiveLimit,
                Items = items
            };
        }
    }
}
