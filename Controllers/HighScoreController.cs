using Microsoft.AspNetCore.Mvc;
using Workshop.Services;

namespace Workshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighScoreController : Controller
    {
        readonly IHighScoreService highScoreService;

        public HighScoreController(IHighScoreService highScoreService) => this.highScoreService = highScoreService;

        [HttpGet]
        public ActionResult<int> Get() => highScoreService.HighScore; 
    }
}