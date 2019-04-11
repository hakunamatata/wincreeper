using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Configuration
{
    public interface IServiceConfig
    {
        string Name { get; }
        string Description { get; }
        string Type { get; }
        int Period { get; }
    }
}
