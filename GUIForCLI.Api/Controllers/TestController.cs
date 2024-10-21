using CliWrap;
using CliWrap.Buffered;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GUIForCLI.Api.Controllers
{

    public class RequestMdl
    {
        public string Command { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestWrapper(RequestMdl request)
        {
            var result = await Cli.Wrap("cmd")
                .WithArguments($"/c{request.Command}")
                .ExecuteBufferedAsync();

            var ret = new
            {
                disks = ParseDiskDriveInfo(result.StandardOutput),
                res = result,
            };

            return Ok(ret);
        }

        static List<DiskDriveInfo> ParseDiskDriveInfo(string wmicOutput)
        {
            List<DiskDriveInfo> diskDrives = new List<DiskDriveInfo>();

            string[] lines = wmicOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines.Skip(1).SkipLast(1)) // Skip the header line
            {
                string[] parts = Regex.Split(line, @"\s{2,}");

                DiskDriveInfo driveInfo = new DiskDriveInfo
                {
                    Caption = parts[0],
                    DeviceID = parts[1],
                    Model = parts[2],
                    Partitions = int.Parse(parts[3]),
                    Size = long.Parse(parts[4])
                };

                diskDrives.Add(driveInfo);
            }

            return diskDrives;
        }
    }

}

