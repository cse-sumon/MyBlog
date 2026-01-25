using Microsoft.AspNetCore.Mvc;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is calling......");
        }
    }
}
