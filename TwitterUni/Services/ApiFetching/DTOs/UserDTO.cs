using Newtonsoft.Json;

namespace TwitterUni.Services.ApiFetching.DTOs
{
    public class UserDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("birthDate")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("profilePic")]
        public string ProfilePic { get; set; }

        [JsonProperty("backgroundPic")]
        public string BackgroundPhoto { get; set; }
    }
}
