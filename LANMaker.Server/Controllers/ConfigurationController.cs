using LANMaker.Data;
using LANMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace LANMaker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly StateContainer stateContainer;
        private readonly ConfigurationService configurationService;

        public ConfigurationController(
            ILogger<ConfigurationController> logger,
            StateContainer stateContainer,
            ConfigurationService configurationService)
        {
            _logger = logger;
            this.stateContainer = stateContainer;
            this.configurationService = configurationService;
        }

        [HttpGet]
        public async Task<Configuration> Get()
        {
            stateContainer.Configuration ??= await configurationService.GetConfiguration(CancellationToken.None);

            return stateContainer.Configuration;
        }
    }
}
