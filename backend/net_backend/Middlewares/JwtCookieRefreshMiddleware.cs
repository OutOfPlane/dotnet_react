using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtCookieRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public JwtCookieRefreshMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        var jwtToken = context.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(jwtToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? "dummy");

            try
            {
                var secret = new SymmetricSecurityKey(key);
                var principal = tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secret,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwt = (JwtSecurityToken)validatedToken;
                var expires = jwt.ValidTo;
                var timeRemaining = expires - DateTime.UtcNow;

                if (timeRemaining > TimeSpan.Zero && timeRemaining < TimeSpan.FromMinutes(15))
                {
                    var token = new JwtSecurityToken(
                        issuer: jwtSettings["Issuer"],
                        audience: jwtSettings["Audience"],
                        claims: principal.Claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: new SigningCredentials(secret, SecurityAlgorithms.HmacSha256)
                    );

                    context.Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        await _next(context);
    }
}