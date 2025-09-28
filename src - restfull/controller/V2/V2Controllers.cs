using Microsoft.AspNetCore.Mvc;
using Routes;

[Route(ApiRoutes.V2)]
[ApiController]
public class V2Controller : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse> GetV2()
    {
        var response = new ApiResponse(
            Status: "ok",
            Version: "2.0.1",
            Timestamp: DateTime.UtcNow
        );

        return Ok(response);
    }
}