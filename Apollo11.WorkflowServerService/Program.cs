using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Apollo11.WorkflowServerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {



            Apollo11WorkflowServer svc = new Apollo11WorkflowServer();
            svc.Start();
            Console.ReadLine();
      //     svc.Stop();

//  this is to tun as service

//            ServiceBase[] ServicesToRun;
  //          ServicesToRun = new ServiceBase[]
    //        {
      //          new Apollo11WorkflowServer()
        //    };
         //   ServiceBase.Run(ServicesToRun);


        }
    }
}
