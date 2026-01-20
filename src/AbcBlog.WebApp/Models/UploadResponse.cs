using System.Text.Json.Serialization;

namespace AbcBlog.WebApp.Models
{
    public class UploadResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
