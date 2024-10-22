using CliWrap;
using CliWrap.Buffered;
using Microsoft.AspNetCore.Mvc;
using PRS = GUIForCLI.Parsers;

namespace GUIForCLI.Api.Controllers
{
    [Route("api/disk")]
    [ApiController]
    public class DiskController : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetPhysicalDisks()
        {
            var result = await Cli.Wrap("bash")
               .WithArguments("-c sudo lshw -class disk")
               .ExecuteBufferedAsync();

            var ret = new
            {
                disks = PRS.DriveInfoParser.ParsePhysicalDisks(result.StandardOutput),
                res = result,
            };
            return Ok(ret);
        }
    }
}

