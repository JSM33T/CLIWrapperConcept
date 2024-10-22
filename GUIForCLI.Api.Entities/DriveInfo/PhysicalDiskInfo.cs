using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Api.Entities.DriveInfo
{
    public class PhysicalDiskInfo
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
}
