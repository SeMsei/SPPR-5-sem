using Domain.Entities;

namespace BlazorWasm.Services;

public interface IDataService
{
	event Action DataChanged;
	List<VideoGameCategory>? Categories { get; set; }
	List<VideoGame>? VGList { get; set; }
	bool Success { get; set; }
	string? ErrorMessage { get; set; }
	int TotalPages { get; set; }
	int CurrentPage { get; set; }

	public Task GetVGListAsync(string? categoryNormalizedName, int pageNo = 1);

	public Task<VideoGame?> GetVGByIdAsync(int id);

	public Task GetCategoryListAsync();
}