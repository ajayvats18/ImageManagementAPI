using System.Text.RegularExpressions;
using System;

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
	public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string imageDetails)
	{
		var model = ValidateImageModel(imageDetails);
		if (file?.Length == 0 || model == null)
		{
			return BadRequest("Please select valid file to be uploaded and check the image model.");
		}

		var response = await _repo.SaveImage(JsonSerializer.Deserialize<ImageModel>(imageDetails), file);
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

	private ImageModel ValidateImageModel(string imageModelString)
	{
		try
		{
			var model = JsonSerializer.Deserialize<ImageModel>(imageModelString);
			string urlPattern = @"^(https?:\/\/)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$";
			if (string.IsNullOrWhiteSpace(model?.User)
			   || string.IsNullOrWhiteSpace(model?.Description)
			   || string.IsNullOrWhiteSpace(model?.Url)
			   || !Regex.IsMatch(model?.Url, urlPattern)
			  )
				return null;

			return model;
		}
		catch (Exception)
		{
			return null;
		}
	}
}