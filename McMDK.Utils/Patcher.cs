using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace McMDK.Utils
{
    public class Patcher
    {
        public static void ApplyPatch(string dir, string works)
        {
            if (!FileController.Exists(dir))
            {
                return;
            }
            List<string> files = Directory.GetFiles(dir, "*.patch", SearchOption.AllDirectories).ToList();
            foreach (var file in files)
            {
                var sr = new StreamReader(file);
                string line, diff = "";
                while ((line = sr.ReadLine()) != null)
                {
                    diff += line + Environment.NewLine;
                }
                sr.Close();
                sr.Dispose();

                var sw = new StreamWriter(works + "\\temp.patch");
                sw.WriteLine(diff);
                sw.Close();
                sw.Dispose();

                string args = "-p1 -u -i \"{0}\"";
                Define.GetLogger().Info("Apply patch from " + file);
                Define.GetLogger().Debug("Arguments - " + String.Format(args, works + "\\temp.patch"));

                var process = new Process();
                process.StartInfo.FileName = works + "\\runtime\\bin\\applydiff.exe";
                process.StartInfo.Arguments = String.Format(args, works + "\\temp.patch");
                process.StartInfo.WorkingDirectory = works;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, eventArgs) => Define.GetLogger().Info(eventArgs.Data);
                process.ErrorDataReceived += (sender, eventArgs) => Define.GetLogger().Error(eventArgs.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
