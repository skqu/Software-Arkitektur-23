using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Routes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route(ApiRoutes.Auth)]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly string  _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;

    public AuthController(IConfiguration config)
    {
        _issuer = config["Jwt:Issuer"];
        _audience = config["Jwt:Audience"];
        _signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:SigningKey"]));
    }

    [HttpPost("login")]
    public ActionResult<object> Login([FromBody] LoginRequest req)
    {
        if (!Users.users.Values.Any(u => u.Uname == req.Username && u.Pwd == req.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        var user = Users.users.Values.First(u => u.Uname == req.Username && u.Pwd == req.Password);

        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Uname)
        };

        if (user.Role == "admin")
        {
            claims.Add(new Claim("scope", "api.read api.admin"));
        }
        else if (user.Role == "user")
        {
            claims.Add(new Claim("scope", "api.read"));
        }
        else
        {
            return BadRequest("User role is invalid");
        }

        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now,
            expires: now.AddHours(1),
            signingCredentials: creds
        );

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            access_token = jwtString,
            token_type = "Bearer",
            expires_in = 3600,
            role = user.Role
        });
    }
}

public record LoginRequest(string Username, string Password);
