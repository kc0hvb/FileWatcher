using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    static class Program
    {
        /// <summary>
        /// The Main entry point for the application
        /// </summary>
        static void Main()
        {
#if DEBUG
            Service1 myservice = new Service1();
            myservice.OnDebug();

#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
            ServiceController sc = new ServiceController();
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                // Start the service if the current status is stopped.
                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Could not start the Alerter service.");
                }
            }

#endif
        }
    }

}
