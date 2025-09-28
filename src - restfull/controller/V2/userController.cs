using Microsoft.AspNetCore.Mvc;
using Routes;

[Route(ApiRoutes.v2Users)]
[ApiController]
public class UserV2Controller : ControllerBase
{

    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] RecordedUser usr)
    {
        User newusr = Users.UserId(usr);
        Users.usersV2[newusr.Id] = newusr;

        return newusr;
    }

    [HttpGet]
    public ActionResult<List<Guid>> ReadUsers()
    {
        var users = Users.usersV2.Values.ToList();
        return Ok(users);
    }

}