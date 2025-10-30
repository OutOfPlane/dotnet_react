using TypeGen.Core.TypeAnnotations;

namespace Models
{
    [ExportTsClass]
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public string? Unit { get; set; }


    }
}

