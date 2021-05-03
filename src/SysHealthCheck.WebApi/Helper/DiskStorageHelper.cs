using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHealthCheck.WebApi.Helper
{
    public class DiskStorageHelper
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Drive { get; set; }
        public int MinFree { get; set; }
    }
}
