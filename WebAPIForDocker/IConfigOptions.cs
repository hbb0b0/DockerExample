using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForDocker
{
    public interface IConfigOptions
    {
        string ConnectionString
        {
            get;
            set;
        }
    }
}
