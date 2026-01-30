using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Dtos;
using MyBlog.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _service;
        private readonly FileService _fileService;

        public BlogController(BlogService service, FileService fileService)
        {
            _service = service;
            _fileService = fileService;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Upload file if provided
            if (file != null)
            {
                try
                {
                    dto.FilePath = await _fileService.UploadFileAsync(file);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            var created = await _service.CreateAsync(dto);
            return Ok(new { message = "Blog created successfully", id = created.Id });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BlogDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Upload new file if provided
            if (file != null)
            {
                try
                {
                    dto.FilePath = await _fileService.UploadFileAsync(file);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Blog updated successfully" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Blog deleted successfully" });
        }
    }
}
