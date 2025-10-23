using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/version")]
    [ApiController]
    public class AppVersionController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public AppVersionController(IWebHostEnvironment env) => _env = env;

        [HttpGet]
        public async Task<IActionResult> GetVersionAsync()
        {
            var path = Path.Combine(_env.WebRootPath, "version.json");
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var json = await System.IO.File.ReadAllTextAsync(path);
            return Content(json, "application/json");
        }
    }
}
