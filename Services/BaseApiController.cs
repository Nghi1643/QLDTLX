using Microsoft.AspNetCore.Mvc;
using QuanLyDaoTaoLaiXe.Components;

namespace QuanLyDaoTaoLaiXe.Services;
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IWebHostEnvironment _hostingEnvironment;
    public BaseApiController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }
    protected ActionResult HandlerResult<T>(Result<T> result)
    {
        //var val = result.Value;
        //if (result == null) return NotFound();
        //if (result.IsSuccess && result.Value != null)
        //    return Ok(result);
        //if (result.IsSuccess && result.Value == null)
        //    return Ok(result);
        //return BadRequest(result.Error);

        if (result.IsSuccess && result.Value == null) return NotFound();
        if (result.IsSuccess && result.Value != null) return Ok(result);
        return BadRequest(result.Error);
    }
}
