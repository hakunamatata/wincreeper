using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Core;

namespace WinApp.Components
{
    public partial class ActionBase : Form
    {
        Thread ProcessingThread;
        ThreadStart processing;
        public ProcessableProgressBar ProgressBar { get; private set; }
        public ActionBase()
        {
            InitializeComponent();
            ControlBox = false;
        }

        public void Process(IProcessable process, Action action)
        {
            ProgressBar = new ProcessableProgressBar(process, action);
            ProgressBar.OnStart += OnStart;
            ProgressBar.OnUpdate += OnUpdate;
            ProgressBar.OnFinished += OnFinished;
            processing = new ThreadStart(action);
            ProcessingThread = new Thread(processing);
            ProcessingThread.Start();
            ShowDialog();

        }

        protected virtual void OnFinished(object sender, ProcessorEventArgs e)
        {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate () {
                    UseWaitCursor = false;
                    DialogResult = DialogResult.OK;
                });
            }
            else {
                UseWaitCursor = false;
                DialogResult = DialogResult.OK;
            }
        }

        protected virtual void OnUpdate(object sender, ProcessorEventArgs e)
        {

        }

        protected virtual void OnStart(object sender, ProcessorEventArgs e)
        {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate () {
                    UseWaitCursor = true;
                    ProgressBar.Dock = DockStyle.Fill;
                    panelProgress.Controls.Add(ProgressBar);
                });
            }
            else {
                UseWaitCursor = true;
                ProgressBar.Dock = DockStyle.Fill;
                panelProgress.Controls.Add(ProgressBar);
            }
        }
    }

}
