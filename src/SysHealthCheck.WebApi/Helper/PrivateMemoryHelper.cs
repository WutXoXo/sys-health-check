using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHealthCheck.WebApi.Helper
{
    public class PrivateMemoryHelper
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public int Maximum { get; set; }
    }
}
