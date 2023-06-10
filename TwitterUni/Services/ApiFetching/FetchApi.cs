using Newtonsoft.Json;
using TwitterUni.Services.ApiFetching.DTOs;

namespace TwitterUni.Services.ApiFetching
{
    public class FetchApi : IFetchApi
    {
        private readonly string _apiUrl;
        private readonly HttpClient _httpClient;

        public FetchApi()
        {
            _httpClient = new HttpClient();
            //_apiUrl = "https://twitter-data.up.railway.app";
            _apiUrl = "http://localhost:3000";
        }

        public async Task<ICollection<UserDTO>> FetchUserData(int count)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();

            string url = _apiUrl + $"/users/{count}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonRes = await response.Content.ReadAsStringAsync();
                userDTOs = JsonConvert.DeserializeObject<UserDTO[]>(jsonRes).ToList();
            }

            return userDTOs;
        }

        public async Task<ICollection<UserPostDTO>> FetchUserPostData(int count, int textLength)
        {
            List<UserPostDTO> tweetDTOs = new List<UserPostDTO>();

            string url = _apiUrl + $"/tweets/{count}/{textLength}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonRes = await response.Content.ReadAsStringAsync();
                tweetDTOs = JsonConvert.DeserializeObject<UserPostDTO[]>(jsonRes).ToList();
            }

            return tweetDTOs;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
