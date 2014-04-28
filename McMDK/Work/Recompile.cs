using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Data;
using McMDK.Utils;
using McMDK.Utils.Log;
using McMDK.ViewModels;

namespace McMDK.Work
{
    public delegate void RecompileFinishedEventHandler(object sender);

    public class Recompile : WorkBase
    {
        private readonly Project Project;
        private readonly string WorkingDirectory;

        public Recompile(Project project)
        {
            this.Project = project;
            this.WorkingDirectory = this.Project.Path;
        }

        public override void Work()
        {
            this.Logging("");
            this.Logging("Recompile Project");
            this.Logging(this.Project.ToString());

            var process = new Process();
            if(this.Project.McpVersion.Equals("gradle"))
            {
                process.StartInfo.FileName = WorkingDirectory + "\\gradlew.bat";
                process.StartInfo.Arguments = "build";
                process.StartInfo.WorkingDirectory = WorkingDirectory;
            }
            else
            {
                process.StartInfo.FileName = WorkingDirectory + "\\runtime\\bin\\python\\python_mcp.exe";
                process.StartInfo.Arguments = "\"" + WorkingDirectory + "\\runtime\\recompile.py\"";
                process.StartInfo.WorkingDirectory = WorkingDirectory;
            }
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += DataReceived;
            process.ErrorDataReceived += DataReceived;
            process.Exited += (sender, args) => OnWorkFinished(this, null);
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                this.Logging(e.Data);
            }
        }

        protected override void OnWorkFinished(object sender, object args)
        {
            base.OnWorkFinished(sender, args);
        }
    }
}
