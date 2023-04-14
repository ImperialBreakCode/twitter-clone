using TwitterUni.Services.ModelData;

namespace TwitterUni.Models
{
    public class HomeViewModel
    {
        public ICollection<UserData> Users { get; set; }
    }
}
