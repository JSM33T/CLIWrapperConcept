using System;
using System.Collections.Generic;
using System.IO;

public class PartitionDetail
{
    public string Name { get; set; }
    public long Size { get; set; } // Size in bytes
    public string Type { get; set; }
}

public class LinuxPartitionReader
{
    public static List<PartitionDetail> GetPartitionDetails()
    {
        var partitions = new List<PartitionDetail>();
        var blockDevices = Directory.GetDirectories("/sys/block");

        foreach (var device in blockDevices)
        {
            var partitionDirs = Directory.GetDirectories(device, "*", SearchOption.TopDirectoryOnly);
            foreach (var partitionDir in partitionDirs)
            {
                var partitionName = Path.GetFileName(partitionDir);
                var sizePath = Path.Combine(partitionDir, "size");
                var typePath = Path.Combine(partitionDir, "queue/rotational");

                if (File.Exists(sizePath))
                {
                    var size = long.Parse(File.ReadAllText(sizePath).Trim()) * 512; // Size in bytes
                    var type = File.Exists(typePath) && File.ReadAllText(typePath).Trim() == "1" ? "HDD" : "SSD";

                    partitions.Add(new PartitionDetail
                    {
                        Name = partitionName,
                        Size = size,
                        Type = type
                    });
                }
            }
        }

        return partitions;
    }
}