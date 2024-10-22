using System.Text.RegularExpressions;
using GUIForCLI.Api.Entities.Zpool;

namespace GUIForCLI.Parsers
{
    public static partial class ZpoolParser
    {
        public static List<ZpoolInfo> ParseZPool(string input)
        {
            var zpools = new List<ZpoolInfo>();
            var zpoolSections = RgxZpool().Split(input).Skip(1); // Split the zpool sections from the output

            foreach (var section in zpoolSections)
            {
                var zpoolInfo = new ZpoolInfo
                {
                    Name = ExtractZpoolInfo(section, "NAME"),
                    Size = ExtractZpoolInfo(section, "SIZE"),
                    Allocated = ExtractZpoolInfo(section, "ALLOC"),
                    Free = ExtractZpoolInfo(section, "FREE"),
                    Fragmentation = ExtractZpoolInfo(section, "FRAG"),
                    Capacity = ExtractZpoolInfo(section, "CAP"),
                    Health = ExtractZpoolInfo(section, "HEALTH")
                };

                zpools.Add(zpoolInfo);
            }

            return zpools;
        }

        private static string ExtractZpoolInfo(string section, string key)
        {
            var match = Regex.Match(section, $@"{Regex.Escape(key)}\s+(\S+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        [GeneratedRegex(@"\s*\bNAME\s+")]
        private static partial Regex RgxZpool();
    }
}
