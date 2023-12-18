using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace web_153501_fomichevskiy.Services.ProductService
{
	public class ApiProductService: IProductService
	{
		HttpClient _httpClient;
		private readonly string _pageSize;
		JsonSerializerOptions _serializerOptions;
		ILogger<ApiProductService> _logger;
		HttpContext _httpContext;

        public ApiProductService(HttpClient httpClient,
			 IConfiguration configuration,
			 ILogger<ApiProductService> logger,
             IHttpContextAccessor httpContextAccessor)
					{
						_httpClient = httpClient;
						_pageSize = configuration.GetSection("ItemsPerPage").Value;
			 _serializerOptions = new JsonSerializerOptions()
			 {
				 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			 };
			_logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }
		public async Task<ResponseData<ListModel<VideoGame>>> GetProductListAsync(
												string? categoryNormalizedName,
												int pageNo = 1)
		{
			// подготовка URL запроса
			var urlString
			= new
			StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}VideoGames/");
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",token);
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
			{
				urlString.Append($"{categoryNormalizedName}/");
			};
			// добавить номер страницы в маршрут
			if (pageNo > 1)
			{
				urlString.Append($"page{pageNo}");
			};
			// добавить размер страницы в строку запроса
			if (!_pageSize.Equals("3"))
			{
				urlString.Append(QueryString.Create("pageSize", _pageSize));
			}

			// отправить запрос к API
			var response = await _httpClient.GetAsync(
			new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				try
				{
					return await response
					.Content
					.ReadFromJsonAsync<ResponseData<ListModel<VideoGame>>>
					(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return new ResponseData<ListModel<VideoGame>>
					{
						Success = false,
						ErrorMessage = $"Ошибка: {ex.Message}"
					};
				}
			}
			_logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
			return new ResponseData<ListModel<VideoGame>>
			{
				Success = false,
				ErrorMessage = $"Данные не получены от сервера. Error: { response.StatusCode.ToString() }"
			};
		}

		public async Task<ResponseData<VideoGame>> CreateProductAsync(VideoGame product,IFormFile? formFile)
		{
			var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "VideoGames");
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync(
			uri,
			product,
			_serializerOptions);
			if (response.IsSuccessStatusCode)
			{
				var data = await response
				.Content
				.ReadFromJsonAsync<ResponseData<VideoGame>>
				(_serializerOptions);
                if (formFile is not null)
                {
                    await SaveImageAsync(data.Data.Id, formFile);
                }

                return data; // dish;
			}
			_logger
			.LogError($"-----> object not created. Error{ response.StatusCode.ToString()}");
			return new ResponseData<VideoGame>
			{
				Success = false,
				ErrorMessage = $"Объект не добавлен. Error: { response.StatusCode.ToString() }"
			};
		}
        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress?.AbsoluteUri}VideoGames/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            await _httpClient.SendAsync(request);
        }
        public async Task<ResponseData<VideoGame>> GetProductByIdAsync(int id)
		{
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}VideoGames/{id}");
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<VideoGame>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<VideoGame>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<VideoGame>()
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}"
            };
        }
		public async Task UpdateProductAsync(int id, VideoGame product, IFormFile? formFile)
		{
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}VideoGames/{id}");
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsync(new Uri(urlString.ToString()),
                new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                if (formFile is not null)
                {
                    int VideoGameId = (await response.Content.ReadFromJsonAsync<ResponseData<VideoGame>>(_serializerOptions)).Data.Id;
                    await SaveImageAsync(VideoGameId, formFile);
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            }
        }
		public async Task DeleteProductAsync(int id)
		{
            var uriString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}VideoGames/{id}");
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.DeleteAsync(new Uri(uriString.ToString()));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            }
        }
	}
}
