using System.ComponentModel.DataAnnotations;

namespace TwitterUni.Models.Tweet
{
    public class CreateTweetViewModel
    {
        public string TextContent { get; set; }

        [Display(Name = "Add Image")]
        public IFormFile Image { get; set; }
    }
}
