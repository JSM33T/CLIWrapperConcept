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

    public class DiskInfo
    {
        public string Description { get; set; }
        public string Product { get; set; }
        public string Vendor { get; set; }
        public string PhysicalId { get; set; }
        public string BusInfo { get; set; }
        public string LogicalName { get; set; }
        public string Version { get; set; }
        public string Serial { get; set; }
        public string Size { get; set; }
        public string Capabilities { get; set; }
        public string Configuration { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestWrapper(RequestMdl request)
        {
            var result = await Cli.Wrap("bash")
               .WithArguments($"-c \"{request.Command}\"")
               .ExecuteBufferedAsync();

            var ret = new
            {
                disks = ParseDisks(result.StandardOutput),
                res = result,
            };
            return Ok(ret);
        }

        [HttpGet]
        public async Task<IActionResult> LinuxPartitions()
        {
             var partitionDetails = LinuxPartitionReader.GetPartitionDetails();
            foreach (var partition in partitionDetails)
            {
                Console.WriteLine($"Name: {partition.Name}, Size: {partition.Size} bytes, Type: {partition.Type}");
            }
            return Ok(partitionDetails);
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

        public static List<DiskInfo> ParseDisks(string input)
        {
            var disks = new List<DiskInfo>();
            var diskSections = Regex.Split(input, @"\s*\*-\s*disk:\d+").Skip(1); // Split the input for each disk section

            foreach (var section in diskSections)
            {
                var diskInfo = new DiskInfo
                {
                    Description = GetValue(section, "description:"),
                    Product = GetValue(section, "product:"),
                    Vendor = GetValue(section, "vendor:"),
                    PhysicalId = GetValue(section, "physical id:"),
                    BusInfo = GetValue(section, "bus info:"),
                    LogicalName = GetValue(section, "logical name:"),
                    Version = GetValue(section, "version:"),
                    Serial = GetValue(section, "serial:"),
                    Size = GetValue(section, "size:"),
                    Capabilities = GetValue(section, "capabilities:"),
                    Configuration = GetValue(section, "configuration:")
                };
                disks.Add(diskInfo);
            }

            return disks;
        }

        private static string GetValue(string section, string key)
        {
            var match = Regex.Match(section, $@"{Regex.Escape(key)}\s*(.+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
    }

}

