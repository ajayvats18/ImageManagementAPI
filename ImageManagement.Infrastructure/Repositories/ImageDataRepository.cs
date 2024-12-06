namespace ImageManagement.Infrastructure.Repositories;

public class ImageDataRepository : IImageDataRepository
{
	private readonly string _filePath;

	public ImageDataRepository()
	{
		_filePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageData.json");
	}

	public async Task<List<ImageModel>> GetAll()
	{
		var json = await File.ReadAllTextAsync(_filePath);
		try
		{
			return JsonSerializer.Deserialize<List<ImageModel>>(json) ?? new List<ImageModel>();
		}
		catch
		{
			return new List<ImageModel>();
		}
	}

	public async Task<ImageModel> GetImageById(int id)
	{
		var json = await File.ReadAllTextAsync(_filePath);
		var data = JsonSerializer.Deserialize<List<ImageModel>>(json) ?? new List<ImageModel>();
		return data.FirstOrDefault(image => image.Id == id);
	}
	
	public async Task DeleteImageById(int id)
	{
		var json = await File.ReadAllTextAsync(_filePath);
		var data = JsonSerializer.Deserialize<List<ImageModel>>(json) ?? new List<ImageModel>();
		var itemToRemove = data.FirstOrDefault(image => image.Id == id);
		if (itemToRemove == null || itemToRemove?.Id <= 0)
			throw new Exception("Invalid id passed.");
		DeleteFile(itemToRemove.FileName);
		data.Remove(itemToRemove);
		json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
		await File.WriteAllTextAsync(_filePath, json);
	}

	public async Task<string> SaveImage(ImageModel imageData, IFormFile file)
	{
		var imagesData = await GetAll();
		imageData.Id = imagesData.Any() ? imagesData.Max(image => image.Id) + 1 : 1;
		imagesData.Add(imageData);
		string filePath = await SaveFile(file);
		imageData.FileName = filePath;
		await Save(imagesData);
		return filePath;
	}

	private async Task Save(List<ImageModel> imageData)
	{
		var json = JsonSerializer.Serialize(imageData, new JsonSerializerOptions { WriteIndented = true });
		await File.WriteAllTextAsync(_filePath, json);
	}

	private async Task<string> SaveFile(IFormFile file)
	{
		var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

		// Ensure the directory exists
		if (!Directory.Exists(uploadDirectory))
		{
			Directory.CreateDirectory(uploadDirectory);
		}

		// Create a unique file name to avoid conflicts
		var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

		// Full path to save the file
		var filePath = Path.Combine(uploadDirectory, fileName);

		// Save the file
		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}
		return filePath;
	}
	
	private void DeleteFile(string fileName)
	{
		// Ensure the file exists
		if (!File.Exists(fileName))
		{
			throw new Exception("Upload directory or File does not exist");
		}

		File.Delete(fileName);
	}
}