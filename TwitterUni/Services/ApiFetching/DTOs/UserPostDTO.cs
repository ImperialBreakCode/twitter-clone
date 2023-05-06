using Newtonsoft.Json;

namespace TwitterUni.Services.ApiFetching.DTOs
{
    public class UserPostDTO
    {
        public UserPostDTO()
        {
            TagNames = new List<string>();
        }

        [JsonProperty("text")]
        public string TextContent { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("tags")]
        public ICollection<string> TagNames { get; set; }
    }
}
