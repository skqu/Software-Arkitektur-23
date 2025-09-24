using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class V1Controller : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse> GetV1()
    {
        var response = new ApiResponse(
            Status: "ok",
            Version: "1.0.1",
            Timestamp: DateTime.UtcNow
        );

        return Ok(response);
    }
}