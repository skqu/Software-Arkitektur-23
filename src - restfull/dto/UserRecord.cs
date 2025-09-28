using Microsoft.AspNetCore.Identity;

public record RecordedUser
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public DateTime CreatedUtc { get; init; } = DateTime.UtcNow;
   // public  Guid  Id { get; set; } = Guid.NewGuid();
    public string Pwd { get; init; } = default!;
    public string Uname { get; init; } = default!;
    public string Role { get; init; } = default!;
};

public record User
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime CreatedUtc { get; set; }
    public  Guid  Id { get; set; } = Guid.NewGuid();
    public string Pwd { get; set; } = default!;
    public string Uname { get; set; } = default!;
    public string Role { get; set; } = default!;
};


static public class Users
{
    static public Dictionary<Guid, User> users = new Dictionary<Guid, User>();
    static public Dictionary<Guid, User> usersV2 = new Dictionary<Guid, User>();
    static public User UserId(RecordedUser usr1)
    {
        User newUsr = new User();
        newUsr.CreatedUtc = usr1.CreatedUtc;
        newUsr.Name = usr1.Name;
        newUsr.Email = usr1.Email;
        newUsr.Pwd = usr1.Pwd;
        newUsr.Uname = usr1.Uname;
        newUsr.Role = usr1.Role;
        return newUsr;
    }
}