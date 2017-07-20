using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Xiaolei.TraceLib;

namespace Xiaolei.ServicePacker
{
    class Program
    {
        static void Main(string[] args)
        {
            TraceHelper.TraceInfo("Xiaolei.ServicePacker main");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MyService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
