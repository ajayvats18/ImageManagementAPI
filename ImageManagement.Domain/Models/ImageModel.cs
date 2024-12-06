namespace ImageManagement.Domain.Models;

public class ImageModel
{
	public int Id { get; set; }
	//[Required]
	public string User { get; set; }
	//[Required]
	//[Url]
	public string Url { get; set; }
	//[Required]
	public string Description { get; set; }
	public string FileName { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
