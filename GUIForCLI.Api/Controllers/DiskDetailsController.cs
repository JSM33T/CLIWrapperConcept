

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CliWrap;
using CliWrap.Buffered;
using System.Text.RegularExpressions;
using GUIForCLI.Api.Entities;

namespace GUIForCLI.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DiskDetailsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestWrapper()
        {
            var diskResult = await Cli.Wrap("cmd")
                .WithArguments("/c wmic diskdrive get caption,deviceid,model,partitions,size /format:csv")
                .ExecuteBufferedAsync();

            var partitionResult = await Cli.Wrap("cmd")
                .WithArguments("/c wmic partition get deviceid,diskindex,size,type /format:csv")
                .ExecuteBufferedAsync();

            var ret = new
            {
                disks = ParseDiskDriveInfo(diskResult.StandardOutput, partitionResult.StandardOutput),
                diskResult = diskResult,
                partitionResult = partitionResult
            };

            return Ok(ret);
        }

        static List<DiskDriveInfo> ParseDiskDriveInfo(string diskOutput, string partitionOutput)
        {
            List<DiskDriveInfo> diskDrives = new List<DiskDriveInfo>();

            // Parse disk information
            var diskLines = diskOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var diskData = diskLines.Skip(1).SkipLast(1).Select(line => line.Split(','));

            foreach (var disk in diskData)
            {
                if (disk.Length < 5) continue;

                DiskDriveInfo driveInfo = new DiskDriveInfo
                {
                    Caption = disk[1],
                    DeviceID = disk[2],
                    Model = disk[3],
                    Partitions = int.Parse(disk[4]),
                    Size = long.Parse(disk[5]),
                    PartitionInfo = new List<PartitionInfo>()
                };

                diskDrives.Add(driveInfo);
            }

            // Parse partition information
            var partitionLines = partitionOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var partitionData = partitionLines.Skip(1).Select(line => line.Split(','));

            foreach (var partition in partitionData)
            {
                if (partition.Length < 5) continue;

                int diskIndex = int.Parse(partition[2]);
                var associatedDisk = diskDrives.FirstOrDefault(d => d.DeviceID.EndsWith($"PHYSICALDRIVE{diskIndex}"));

                if (associatedDisk != null)
                {
                    associatedDisk.PartitionInfo.Add(new PartitionInfo
                    {
                        DeviceID = partition[1],
                        Size = long.Parse(partition[3]),
                        Type = partition[4]
                    });
                }
            }

            return diskDrives;
        }
    }

    public class DiskDriveInfo
    {
        public string Caption { get; set; }
        public string DeviceID { get; set; }
        public string Model { get; set; }
        public int Partitions { get; set; }
        public long Size { get; set; }
        public List<PartitionInfo> PartitionInfo { get; set; }
    }

    public class PartitionInfo
    {
        public string DeviceID { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }
}