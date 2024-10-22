using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Api.Entities.Zpool
{
    public class ZpoolInfo
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Allocated { get; set; }
        public string Free { get; set; }
        public string Fragmentation { get; set; }
        public string Capacity { get; set; }
        public string Health { get; set; }
    }
}
