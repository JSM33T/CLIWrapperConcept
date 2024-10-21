using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Api.Entities
{
    public class DiskDriveInfo
    {
        public string Caption { get; set; }
        public string DeviceID { get; set; }
        public string Model { get; set; }
        public int Partitions { get; set; }
        public long Size { get; set; }
    }
}
