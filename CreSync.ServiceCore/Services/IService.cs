using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore
{
    public interface IService
    {
        string ServiceName { get; }
        string ServiceDescription { get; }
        bool Initialize();
        void Start();
        void Stop();
    }
}
