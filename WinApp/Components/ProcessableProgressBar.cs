using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Core;

namespace WinApp.Components
{
    public class ProcessableProgressBar : ProgressBar
    {
        IProcessable ProcessableBusiness;
        Action ProcessAction;

        public event EventHandler<ProcessorEventArgs> OnStart;

        public event EventHandler<ProcessorEventArgs> OnUpdate;

        public event EventHandler<ProcessorEventArgs> OnFinished;

        public ProcessableProgressBar() : base()
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        public ProcessableProgressBar(IProcessable processable, Action processAction) : this()
        {
            ProcessableBusiness = processable;
            ProcessAction = processAction;

            ProcessableBusiness.OnStart += ProcessableBusiness_OnStart;
            ProcessableBusiness.OnUpdate += ProcessableBusiness_OnUpdate;
            ProcessableBusiness.OnFinished += ProcessableBusiness_OnFinished;
        }

        private void ProcessableBusiness_OnFinished(object sender, ProcessorEventArgs e)
        {
            var process = sender as IProcessable;
            Value = e.Progress;
            OnFinished?.Invoke(process, e);
        }

        private void ProcessableBusiness_OnUpdate(object sender, ProcessorEventArgs e)
        {
            var process = sender as IProcessable;
            Value = e.Progress;
            OnUpdate?.Invoke(process, e);
        }

        private void ProcessableBusiness_OnStart(object sender, ProcessorEventArgs e)
        {
            var process = sender as IProcessable;
            Maximum = process.MaxValue;
            Minimum = process.MinValue;
            Value = e.Progress;
            OnStart?.Invoke(process, e);
        }
    }
}
