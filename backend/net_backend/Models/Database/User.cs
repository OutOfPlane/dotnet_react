using TypeGen.Core.TypeAnnotations;

namespace Models
{
    [ExportTsClass]
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        [Sensitive]
        public required string PwHash { get; set; }
    }
}

