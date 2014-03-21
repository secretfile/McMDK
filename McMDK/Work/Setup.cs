using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using McMDK.Data;
using McMDK.Utils;

namespace McMDK.Work
{
    public delegate void SetupFinishedEventHandler(object sender);

    public class Setup
    {
        public readonly Project Project;
        public event SetupFinishedEventHandler OnFinished;
        private List<string> downloads;
        private List<string> files;
        private string workingDirectory;

        public Setup(Project project)
        {
            this.Project = project;
            this.downloads = new List<string>();
            this.files = new List<string>();

            this.workingDirectory = Define.ProjectDirectory + "\\" + this.Project.Name + "\\";
        }


        public void Work()
        {
            Define.GetLogger().Info("");
            Define.GetLogger().Info("Setup Modding Environment");
            Define.GetLogger().Info(this.Project.ToString());

            if (this.Project.McpVersion.Equals("Gradle"))
            {
                this.downloads = new List<string>
                {
                    String.Format(Define.ForgeGradleUrl, this.Project.McVersion.Replace(" Gradle", ""), this.Project.ForgeVersion)
                };
                this.files = new List<string>
                {
                    workingDirectory + "minecraftforge.zip"
                };
            }
            else
            {
                this.downloads = new List<string>
                {
                    String.Format(Define.CoderPackUrl, this.Project.McpVersion),
                    String.Format(Define.ForgeBinaryUrl, this.Project.McVersion, this.Project.ForgeVersion),
                    String.Format(Define.PatchUrl, this.Project.McVersion)
                };
                this.files = new List<string>
                {
                    workingDirectory + "mcp.zip",
                    workingDirectory + "minecraftforge.zip",
                    workingDirectory + "patch.zip"
                };

                if (int.Parse(this.Project.McVersion.Replace(".", "")) < 150)
                {
                    this.downloads.Add(String.Format(Define.McBinaryUrl, this.Project.McVersion.Replace(".", "_")));
                    this.downloads.Add(String.Format(Define.McSrvBinaryUrl, this.Project.McVersion.Replace(".", "_")));
                    this.downloads.Add(Define.McResourcesUrl + "lwjgl.jar");
                    this.downloads.Add(Define.McResourcesUrl + "lwjgl_util.jar");
                    this.downloads.Add(Define.McResourcesUrl + "jinput.jar");
                    this.downloads.Add(Define.McResourcesUrl + "windows_natives.jar");
                    this.downloads.Add(Define.McResourcesUrl + "macosx_natives.jar");
                    this.downloads.Add(Define.McResourcesUrl + "linux_natives.jar");
                    this.downloads.Add(Define.LibrariesUrl + "fernflower.jar");

                    this.files.Add(workingDirectory + "jars\\bin\\minecraft.jar");
                    this.files.Add(workingDirectory + "jars\\minecraft_server.jar");
                    this.files.Add(workingDirectory + "jars\\bin\\lwjgl.jar");
                    this.files.Add(workingDirectory + "jars\\bin\\lwjgl_utils.jar");
                    this.files.Add(workingDirectory + "jars\\bin\\jinput.jar");
                    this.files.Add(workingDirectory + "jars\\bin\\natives\\windows_natives.zip");
                    this.files.Add(workingDirectory + "jars\\bin\\natives\\macosx_natives.zip");
                    this.files.Add(workingDirectory + "jars\\bin\\natives\\linux_natives.zip");
                    this.files.Add(workingDirectory + "runtime\\bin\\fernflower.jar");
                }
            }

            Define.GetLogger().Fine(this.downloads.Count + " files download.");
            this.Download();
        }

        private async void Download()
        {
            var client = new WebClient();
            client.DownloadProgressChanged += DownloadProgressChanged;
            for (int i = 0; i < this.downloads.Count; i++)
            {
                await client.DownloadFileTaskAsync(this.downloads[i], this.files[i]);
            }
            this.Extract();
        }

        private void Extract()
        {
            foreach (var file in this.files)
            {
                if (file.EndsWith(".zip"))
                {
                    try
                    {
                        if (FileController.Exists(workingDirectory))
                        {
                            FileController.Delete(workingDirectory);
                            FileController.CreateDirectory(workingDirectory);
                        }
                        ZipFile.ExtractToDirectory(file, workingDirectory);
                    }
                    catch (Exception e)
                    {
                        Define.GetLogger().Error("Error occurred in Extracting Zip.", e);
                        return;
                    }
                }
            }
            this.SetupDevEnv();
        }

        private void SetupDevEnv()
        {
            Patcher.ApplyPatch(workingDirectory + "\\patches_before", workingDirectory);

            var process = new Process();

            if (this.Project.McpVersion.Equals("Gradle"))
            {
                process.StartInfo.FileName = workingDirectory + "\\gradlew.bat";
                process.StartInfo.Arguments = "setupDevWorkspace";
                process.StartInfo.WorkingDirectory = workingDirectory;
            }
            else
            {
                process.StartInfo.FileName = workingDirectory + "\\runtime\\bin\\python\\python_mcp.exe";
                process.StartInfo.Arguments = "\"" + workingDirectory + "\\forge\\install.py\" --mcp-dir \"" + workingDirectory + "\"";
                process.StartInfo.WorkingDirectory = workingDirectory + "\\forge\\";
            }
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += DataReceived;
            process.ErrorDataReceived += DataReceived;
            process.Exited += (sender, args) => this.OnFinished(this);
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data))
            {
                
            }
        }
    }
}
