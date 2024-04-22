using LANMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace LANMaker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameRunController : ControllerBase
    {
        private readonly ILogger<GameRunController> _logger;
        private readonly GameRunService gameRunService;

        public GameRunController(
            ILogger<GameRunController> logger,
            GameRunService gameRunService)
        {
            _logger = logger;
            this.gameRunService = gameRunService;
        }

        [HttpGet]
        public async Task Run(string name)
        {
            await gameRunService.PlayGame(name);
        }
    }
}
