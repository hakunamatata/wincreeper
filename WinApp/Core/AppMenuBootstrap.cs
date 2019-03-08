using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp.Core
{
    public class AppMenuBootstrap
    {
        private static List<ToolStripMenuItem> Menus;

        private static List<ToolStripMenuItem> Page { get; set; }
        private static List<ToolStripMenuItem> Settings { get; set; }
        public static ToolStripMenuItem PageNew { get; private set; }
        public static ToolStripMenuItem PageSave { get; private set; }
        public static ToolStripMenuItem PageShare { get; private set; }
        public static ToolStripMenuItem SettingConfig { get; private set; }

        public static void Bootstrap(MenuStrip menu)
        {
            Menus = menu.Items.Cast<ToolStripMenuItem>().ToList();
            Page = getMenuItems(Menus.Where(p => p.Name == "pageToolStripMenuItem").Single()).ToList();
            Settings = getMenuItems(Menus.Where(p => p.Name == "settingsToolStripMenuItem").Single()).ToList();

            PageNew = Page.Where(p => p.Name == "newToolStripMenuItem").Single();
            PageSave = Page.Where(p => p.Name == "saveToolStripMenuItem").Single();
            PageShare = Page.Where(p => p.Name == "shareToolStripMenuItem").Single();

            SettingConfig = Settings.Where(p => p.Name == "configToolStripMenuItem").Single();
        }

        private static IEnumerable<ToolStripMenuItem> getMenuItems(ToolStripMenuItem item)
        {
            foreach (var m in item.DropDownItems) {
                if (m.GetType().Equals(typeof(ToolStripMenuItem)))
                    yield return m as ToolStripMenuItem;
            }
        }

    }
}
