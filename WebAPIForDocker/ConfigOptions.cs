using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForDocker
{
    public class ConfigOptions : IConfigOptions
    {
        public string ConnectionString { get; set; }
    }
}
