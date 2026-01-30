using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Dtos;
using MyBlog.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("blog/{blogId}")]
        public async Task<IActionResult> GetByBlogId(int blogId)
        {
            var comments = await _service.GetByBlogIdAsync(blogId);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return Ok(new { message = "Comment created successfully", id = created.Id });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Comment updated successfully" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Comment deleted successfully" });
        }
    }
}
