using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<ActionResult<bool>> AddScore([FromBody] int score) => await highScoreService.AddScore(score);
    }
}