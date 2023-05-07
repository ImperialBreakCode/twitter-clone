using TwitterUni.Services.ApiFetching.DTOs;

namespace TwitterUni.Services.ApiFetching
{
    public interface IFetchApi : IDisposable
    {
        public Task<ICollection<UserDTO>> FetchUserData(int count);
        public Task<ICollection<UserPostDTO>> FetchUserPostData(int count, int textLength);
    }
}
