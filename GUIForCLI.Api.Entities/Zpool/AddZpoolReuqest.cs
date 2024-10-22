using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Api.Entities.Zpool
{
    public class AddZpoolReuqest
    {
        public string LogicalName { get; set; }
        public string PoolName { get; set; }
    }
}
