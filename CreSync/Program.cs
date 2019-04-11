using CreSync.ServiceCore;
using CreSync.ServiceCore.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CreSync
{
    static class Program
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void RunASConsole()
        {
            Console.WriteLine();
            Console.WriteLine("------------Console-----------");
            var boot = new Bootstrap(AppSetting.Current);

            if (boot.Initialize()) {
                boot.Start();
            }
            else {
                Console.WriteLine("service initialize failed.");
            }
        }
        static void RunAsService()
        {
            //ServiceBase[] servicesToRun;
            //servicesToRun = new ServiceBase[] { new MainService() };
            //ServiceBase.Run(servicesToRun);
        }

        static void Main(string[] args)
        {
            if (!Environment.UserInteractive) {
                RunAsService();
                return;
            }

            Console.WriteLine("Welcome to Wowu88 Xml Parser Registeration Program！");
            Console.WriteLine("press any key to continue...");
            Console.WriteLine("-[r]: Run as Console");
            Console.WriteLine("-[i]: Install service");
            Console.WriteLine("-[u]: Uninstall service");

            string cmdArg = string.Empty;

            if (args == null || args.Length < 1) {
                while (true) {
                    Console.WriteLine();
                    cmdArg = Console.ReadKey().KeyChar.ToString();
                    if (Run(cmdArg, null))
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
            else {
                cmdArg = args[0];

                if (!string.IsNullOrEmpty(cmdArg))
                    cmdArg = cmdArg.TrimStart('-');

                Run(cmdArg, args);
            }

        }

        static bool Run(string cmdArg, string[] args)
        {
            switch (cmdArg.ToLower()) {
                default:
                    return false;

                case "i":
                    SelfInstall.Install();
                    return true;

                case "u":
                    SelfInstall.Uninstall();
                    return true;

                case "r":
                    RunASConsole();
                    return false;
            }
        }
    }
}
