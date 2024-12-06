namespace ImageManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageManagementAPIController : ControllerBase
{
	private readonly IImageDataRepository _repo;
	private readonly IWebHostEnvironment _env;

	public ImageManagementAPIController(IImageDataRepository repo)
	{
		_repo = repo;
	}

	[HttpGet("GetAll")]
	public async Task<IActionResult> GetAllImagesDetails()
	{
		var images = await _repo.GetAll();
		return Ok(images);
	}
	
	[HttpGet("GetImageById/{id}")]
	public async Task<IActionResult> GetImageDetailsById(int id)
	{
		var images = await _repo.GetImageById(id);
		return Ok(images);
	}

	[HttpPost("Upload")]
	public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] ImageModel imageDetails)
	{
		if (file?.Length == 0)
		{
			return BadRequest("Please select file to be uploaded.");
		}
		var response = await _repo.SaveImage(imageDetails, file);
		return CreatedAtAction(nameof(UploadImage), response);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteImage(int id)
	{
		if (id <= 0)
		{
			return BadRequest("Please provide correct id.");
		}

		await _repo.DeleteImageById(id);

		return NoContent();
	}
}