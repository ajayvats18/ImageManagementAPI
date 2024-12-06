namespace ImageManagement.Domain.Repositories;

public interface IImageDataRepository
{
	Task<List<ImageModel>> GetAll();
	Task<ImageModel> GetImageById(int id);
	Task DeleteImageById(int id);
	Task<string> SaveImage(ImageModel imageData, IFormFile file);
}
