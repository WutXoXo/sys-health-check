using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHealthCheck.WebApi.Helper
{
    public class DatabaseHelper
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string ConnectString { get; set; }
        public int Timeout { get; set; }
    }
}
