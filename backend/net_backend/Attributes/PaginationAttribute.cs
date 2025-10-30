using Microsoft.AspNetCore.Mvc;

namespace CustomAttributes
{
    public class PaginationParams
    {
        [FromQuery(Name = "offset")]
        public int? Offset { get; set; }

        [FromQuery(Name = "limit")]
        public int? Limit { get; set; }

        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        [FromQuery(Name = "pagesize")]
        public int? PageSize { get; set; }

        public int EffectiveOffset => Offset ?? ((Page ?? 1) - 1) * (PageSize ?? 10);
        public int EffectiveLimit => Limit ?? PageSize ?? 10;
    }
}
