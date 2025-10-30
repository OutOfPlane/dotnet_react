using TypeGen.Core.TypeAnnotations;

namespace RequestObjects
{
    [ExportTsClass]
    public class Login
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; }
    }
}