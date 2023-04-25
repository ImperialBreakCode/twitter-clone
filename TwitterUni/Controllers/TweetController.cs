using Microsoft.AspNetCore.Mvc;

namespace TwitterUni.Controllers
{
    public class TweetController : Controller
    {
        public IActionResult Index(string id)
        {
            return View();
        }
    }
}
