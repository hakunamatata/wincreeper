using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Core;

namespace WinApp.Components
{
    public partial class ConfigForm : Form
    {
        public AppConfiguration Configuration { get; private set; } = AppConfiguration.Current;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            textBoxLocalRootDirectory.Text = Configuration.AppSettings.SaveConfig.RootLocalDirectory;
            textBoxRemoteDirectory.Text = Configuration.AppSettings.SaveConfig.RemoteDirectory;
            checkBoxIndividule.Checked = Configuration.AppSettings.SaveConfig.LocalIndividulelyDirecotry;
        }

        private void linkLabelBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    textBoxLocalRootDirectory.Text = dialog.SelectedPath;
                    Configuration.AppSettings.SaveConfig.RootLocalDirectory = dialog.SelectedPath;
                }
            }
        }

        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            Configuration.UpdateConfig();
            DialogResult = DialogResult.OK;
        }
    }
}
