using Microsoft.AspNetCore.Mvc;

namespace CustomAttributes
{
    public class FilterRule
    {
        public string? Field { get; set; }
        public string? Op { get; set; }
        public string? Value { get; set; }
        public List<FilterRule> Sub { get; set; } = [];
    }

}
