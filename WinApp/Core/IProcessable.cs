using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApp.Components;

namespace WinApp.Core
{
    public class ProcessorEventArgs
    {
        public int Progress { get; set; } = 0;
        public object Output { get; set; }
        public ProcessorEventArgs(int progress, object output)
        {
            Progress = progress;
            Output = output;
        }
    }
    public interface IProcessable
    {
        event EventHandler<ProcessorEventArgs> OnStart;

        event EventHandler<ProcessorEventArgs> OnUpdate;

        event EventHandler<ProcessorEventArgs> OnFinished;
        ProcessableProgressBar ProcessingUI { get; }
        bool Finished { get; }
        int Progress { get; }
        int MaxValue { get; }
        int MinValue { get; }
        void Ready();
        void Update(int progress);
    }
}
