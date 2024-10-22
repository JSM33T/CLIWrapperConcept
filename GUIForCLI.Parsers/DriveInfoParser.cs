using GUIForCLI.Api.Entities.DriveInfo;
using System.Text.RegularExpressions;

namespace GUIForCLI.Parsers
{
    public static partial class DriveInfoParser
    {
        public static List<PhysicalDiskInfo> ParsePhysicalDisks(string input)
        {
            var disks = new List<PhysicalDiskInfo>();
            //split the disk sections from the output
            var diskSections = RgxPhysicalDisk().Split(input).Skip(1);

            foreach (var section in diskSections)
            {
                var diskInfo = new PhysicalDiskInfo
                {
                    Description = ExtractPhysicalDiskInfo(section, "description:"),
                    Product = ExtractPhysicalDiskInfo(section, "product:"),
                    Vendor = ExtractPhysicalDiskInfo(section, "vendor:"),
                    PhysicalId = ExtractPhysicalDiskInfo(section, "physical id:"),
                    BusInfo = ExtractPhysicalDiskInfo(section, "bus info:"),
                    LogicalName = ExtractPhysicalDiskInfo(section, "logical name:"),
                    Version = ExtractPhysicalDiskInfo(section, "version:"),
                    Serial = ExtractPhysicalDiskInfo(section, "serial:"),
                    Size = ExtractPhysicalDiskInfo(section, "size:"),
                    Capabilities = ExtractPhysicalDiskInfo(section, "capabilities:"),
                    Configuration = ExtractPhysicalDiskInfo(section, "configuration:")
                };
                disks.Add(diskInfo);
            }

            return disks;
        }

        private static string ExtractPhysicalDiskInfo(string section, string key)
        {
            var match = Regex.Match(section, $@"{Regex.Escape(key)}\s*(.+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        [GeneratedRegex(@"\s*\*-\s*disk:\d+")]
        private static partial Regex RgxPhysicalDisk();
    }
}
