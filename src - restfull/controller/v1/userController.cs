using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route(ApiRoutes.User)]
[ApiController]
public class UserController : ControllerBase
{
    private readonly string  _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;

    public UserController(IConfiguration config)
    {
        _issuer = config["Jwt:Issuer"];
        _audience = config["Jwt:Audience"];
        _signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:SigningKey"]));
    }

    [HttpPost]
    public ActionResult<object> CreateUser([FromBody] RecordedUser usr)
    {
        User newusr = Users.UserId(usr);
        Users.users[newusr.Id] = newusr;
        var claims = new List<Claim>
        {
            new("sub", newusr.Id.ToString())
        };

        var now = DateTime.UtcNow;

        if (newusr.Role == "user")
        {
            claims.Add(new Claim("scope", "api.read"));
        }
        else if (newusr.Role == "admin")
        {
            claims.Add(new Claim("scope", "api.read api.admin"));
        }
        else
        {
            return BadRequest("Unknown role");
        }
        

        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience:_audience,
            claims: claims,
            notBefore: now,
            expires: now.AddHours(1),
            signingCredentials: creds);

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

        // return 201 Created with link to GET/{id}, plus token payload
        return CreatedAtAction(nameof(ReadUser), new { id = newusr.Id }, new
        {
            user = newusr,
            access_token = jwtString,
            token_type = "Bearer",
            expires_in = 3600
        });
    }

    [HttpGet]
    [Authorize(Policy = "Scope:api.read")]
    public ActionResult<List<Guid>> ReadUsers()
    {
        var ids = Users.users.Keys.ToList();
        return Ok(ids);
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "Scope:api.read")]
    public ActionResult<User> ReadUser(Guid id)
    {
        return Users.users.TryGetValue(id, out var user)
            ? Ok(user)
            : NotFound();   
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "Scope:api.read")]
    public ActionResult<User> UpdateUser(Guid id, User usr)
    {
        if (id == usr.Id)
        {
            Users.users.Remove(id);
            Users.users[id] = usr;
            return Users.users[id];
        }
        return BadRequest();
    } 
    
    [HttpDelete("{id:Guid}")]
    [Authorize(Policy = "Scope:api.admin")]
    public ActionResult DeleteUser(Guid id)
    {
        Users.users.Remove(id);
        return Ok();
    }
}