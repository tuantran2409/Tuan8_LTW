using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookWinForms.Models;

namespace BookWinForms.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "http://localhost:9999/api/";

        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:9999/"); // Set base to host so we can hit /api and /Content
        }

        public async Task<System.Drawing.Image?> DownloadImageAsync(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            try
            {
                var bytes = await _client.GetByteArrayAsync($"Content/ImageBooks/{fileName}");
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _client.GetAsync("api/books");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Book>>(content);
            }
            return new List<Book>();
        }

        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            var response = await _client.GetAsync($"api/books/search?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Book>>(content);
            }
            return new List<Book>();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _client.GetAsync("api/categories");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Category>>(content);
            }
            return new List<Category>();
        }

        public async Task<string> UploadImageAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return null;

            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(System.IO.File.OpenRead(filePath));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/" + System.IO.Path.GetExtension(filePath).Replace(".", ""));
                content.Add(fileContent, "file", System.IO.Path.GetFileName(filePath));

                var response = await _client.PostAsync("api/books/upload", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeAnonymousType(result, new { fileName = "" });
                    return obj.fileName;
                }
            }
            return null;
        }

        public async Task<(bool success, string message)> AddBookAsync(Book book)
        {
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/books", content);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Thành công");
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Lỗi {response.StatusCode}: {error}");
        }
    }
}
