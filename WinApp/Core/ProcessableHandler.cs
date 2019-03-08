using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApp.Components;

namespace WinApp.Core
{
    public class ProcessableHandler : IProcessable
    {
        public event EventHandler<ProcessorEventArgs> OnStart;
        public event EventHandler<ProcessorEventArgs> OnUpdate;
        public event EventHandler<ProcessorEventArgs> OnFinished;
        public ProcessableProgressBar ProcessingUI { get; private set; }
        public bool Finished { get; private set; } = false;
        public int Progress { get; private set; } = 0;
        public int MaxValue { get; private set; } = int.MaxValue;
        public int MinValue { get; private set; } = 0;

        public ProcessableHandler(ProcessableProgressBar progressBar)
        {
            ProcessingUI = progressBar;
        }
        public void Ready(int progress, int min, int max)
        {
            Finished = false;
            Progress = progress;
            MinValue = min;
            MaxValue = max;
            OnStart?.Invoke(this, new ProcessorEventArgs(0, null));
        }
        public void Ready(int max)
        {
            Ready(0, 0, max);
        }

        public void Ready()
        {
            Ready(0, 0, 100);
        }

        public void Update(int progress)
        {
            Progress += progress;
            if (Progress >= MaxValue) {
                Progress = MaxValue;
                OnFinished?.Invoke(this, new ProcessorEventArgs(Progress, null));
                Finished = true;
            }
            else {
                OnUpdate?.Invoke(this, new ProcessorEventArgs(Progress, null));
            }
        }
    }
}
