using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils;
using McMDK.ViewModels;
using McMDK.Utils.Log;

namespace McMDK.Work
{
    public delegate void WorkFinishedEventHandler(object sender, object args);

    public abstract class WorkBase
    {
        public ProgressWindowViewModel ProgressWindowViewModel;
        public event WorkFinishedEventHandler OnFinished;

        protected void Logging(string text, LogLevel level = LogLevel.INFO, Exception e = null)
        {
            Define.GetLogger().Write(level, text, e);
            if(this.ProgressWindowViewModel != null)
            {
                this.ProgressWindowViewModel.ProgressText = text;
            }
        }

        protected virtual void OnWorkFinished(object sender, object args)
        {
            WorkFinishedEventHandler handler = OnFinished;
            if(handler != null)
            {
                OnFinished(sender, args);
            }
        }

        public abstract void Work();
    }
}
