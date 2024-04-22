using LANMaker.Data;
using LANMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace LANMaker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManifestController : ControllerBase
    {
        private readonly ILogger<ManifestController> _logger;
        private readonly StateContainer stateContainer;
        private readonly ManifestService manifestService;

        public ManifestController(
            ILogger<ManifestController> logger,
            StateContainer stateContainer,
            ManifestService manifestService)
        {
            _logger = logger;
            this.stateContainer = stateContainer;
            this.manifestService = manifestService;
        }

        [HttpGet]
        public async Task<Manifest> Get()
        {
            stateContainer.Manifest ??= await manifestService.GetManifest(CancellationToken.None);

            return stateContainer.Manifest;
        }
    }
}
