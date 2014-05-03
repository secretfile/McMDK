using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using Livet;
using McMDK.Data;
using McMDK.Utils;
using McMDK.Plugin;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
// ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
            this.Initialize();
        }

        private void Initialize()
        {
            try
            {
                string skey = @"Software\JavaSoft\Java Development Kit";
                RegistryKey registryKey;
                if (Environment.Is64BitOperatingSystem)
                {
                    registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(skey);
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey(skey);
                }
// ReSharper disable once PossibleNullReferenceException
                var version = (string) registryKey.GetValue("CurrentVersion");
                registryKey.Close();

                skey = @"Software\JavaSoft\Java Development Kit\" + version;
                if (Environment.Is64BitOperatingSystem)
                {
                    registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(skey);
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey(skey);
                }
// ReSharper disable once PossibleNullReferenceException
                var location = (string) registryKey.GetValue("JavaHome");
                registryKey.Close();

                location += "\\bin\\javac.exe";

                var process = new Process();
                process.StartInfo.FileName = location;
                process.StartInfo.Arguments = "-version";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
            catch (Exception)
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "McMDK エラー";
                taskDialog.InstructionText = "JDKがみつかりませんでした。";
                taskDialog.Text = "JDKを見つけることができませんでした。\nJDKがインストールされていないか、レジストリが正常に設定されていない可能性があります。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (sender, args) =>
                {
                    var dialog = (TaskDialog) sender;
                    dialog.Icon = dialog.Icon;
                };
                taskDialog.Show();

                Environment.Exit(0);
            }

            //設定読み込み
            Settings.Load();

            //読み込み
            Minecraft.Load();
            PluginLoader.Load();
            ProjectLoader.Load();

            //フォルダ作成
            FileController.CreateDirectory(Define.ProjectDirectory);
            FileController.CreateDirectory(Define.TempDirectory);
            FileController.CreateDirectory(Define.PluginDirectory);
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO:ロギング処理など
            MessageBox.Show(
                "不明なエラーが発生しました。アプリケーションを終了します。",
                "エラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        
            Environment.Exit(1);
        }

        protected override void OnExit(ExitEventArgs e)
        {

            base.OnExit(e);
        }
    }
}
