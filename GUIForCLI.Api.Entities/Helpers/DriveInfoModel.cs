using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Api.Entities.Helpers
{
    public class DriveInfoModel
    {
        public string? Drive { get; set; }
        public long Size { get; set; }
        public long FreeSpace { get; set; }
        public string Type { get; set; }
    }
}