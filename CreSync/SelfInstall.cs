using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CreSync
{
    public static class SelfInstall
    {
        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;

        public static bool Install()
        {
            try {
                ManagedInstallerClass.InstallHelper(new string[] { _exePath });
                return true;
            }
            catch {
                return false;
            }

        }

        public static bool Uninstall()
        {
            try {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", _exePath });
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
