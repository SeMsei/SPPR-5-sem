using Domain.Entities;
using Domain.Models;
using System.Text.Json;
using System.Text;

namespace web_153501_fomichevskiy.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<ResponseData<List<VideoGameCategory>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}VideoGameCategories/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var a = response.Content.ReadFromJsonAsync<ResponseData<List<VideoGameCategory>>>(_jsonSerializerOptions).ToString();
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<VideoGameCategory>>>(_jsonSerializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<VideoGameCategory>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };

                }
            }
            _logger.LogError($"-----> КДанные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<List<VideoGameCategory>>()
            {
                Success = false,
                ErrorMessage = $"КДанные не получены от сервера. Error:{response.StatusCode}"
            };
        }
    }
}
