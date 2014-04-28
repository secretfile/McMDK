using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK.Models;
using McMDK.Data;
using McMDK.Work;
using McMDK.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK.ViewModels
{
    public class InformationWindowViewModel : ViewModel
    {

        private MainWindowViewModel MainWindowViewModel;
        private ProgressWindowViewModel ProgressWindowViewModel;

        public InformationWindowViewModel(MainWindowViewModel main, ProgressWindowViewModel progress)
        {
            this.MainWindowViewModel = main;
            this.ProgressWindowViewModel = progress;
            this.MinecraftVersions = Minecraft.MinecraftVersions;
            this.IsShow = false;
        }

        public void Show()
        {
            this.ProjectName = this.MainWindowViewModel.CurrentProject.Name;
            this.ProjectDir = this.MainWindowViewModel.CurrentProject.Path;
            this.MinecraftVersion = this.MainWindowViewModel.CurrentProject.McVersion;
            this.MinecraftForgeVers = Minecraft.ForgeVersions[this.MinecraftVersion];
            this.MinecraftForgeVer = this.MainWindowViewModel.CurrentProject.ForgeVersion;
            this.TextThickness = new System.Windows.Thickness(0);
            this.IsEditable = false;
            this.IsShow = true;
        }


        #region OKButtonCommand
        private ViewModelCommand _OKButtonCommand;

        public ViewModelCommand OKButtonCommand
        {
            get
            {
                if (_OKButtonCommand == null)
                {
                    _OKButtonCommand = new ViewModelCommand(OKButton);
                }
                return _OKButtonCommand;
            }
        }

        public void OKButton()
        {
            bool flag = false, name = false;
            string old_name = this.MainWindowViewModel.CurrentProject.Name;
            if (this.MainWindowViewModel.CurrentProject.McVersion != this.MinecraftVersion)
            {
                flag = true;
            }
            if (this.MainWindowViewModel.CurrentProject.ForgeVersion != this.MinecraftForgeVer)
            {
                flag = true;
            }
            if (this.MainWindowViewModel.CurrentProject.Name != this.ProjectName)
            {
                flag = true;
                name = true;
            }

            //Set
            this.MainWindowViewModel.CurrentProject.Name = this.ProjectName;
            this.MainWindowViewModel.CurrentProject.Path = this.ProjectDir;
            this.MainWindowViewModel.CurrentProject.McVersion = this.MinecraftVersion;
            this.MainWindowViewModel.CurrentProject.ForgeVersion = this.MinecraftForgeVer;
            this.MainWindowViewModel.CurrentProject.McpVersion = Minecraft.MCPVersions[this.MinecraftVersion];

            if(flag)
            {
                this.ProgressWindowViewModel.IsShow = true;
                this.ProgressWindowViewModel.IsImmediate = true;
                this.ProgressWindowViewModel.ProgressText = "プロジェクトをクリーンアップしています。";
                Setup setup = new Setup(this.MainWindowViewModel.CurrentProject);
                setup.ProgressWindowViewModel = this.ProgressWindowViewModel;
                setup.OnFinished += SetupOnFinished;
                if(name)
                {
                    FileController.Delete(Define.ProjectDirectory + "\\" + old_name);
                    FileController.CreateDirectory(Define.ProjectDirectory + "\\" + this.ProjectName);
                }
                string[] files = FileController.LoadDirectory(Define.ProjectDirectory + "\\" + this.ProjectName, false);
                foreach(string file in files)
                {
                    if (file.EndsWith(".json") || file.Contains("project"))
                    {
                        continue;
                    }
                    FileController.Delete(file);
                }
                this.ProgressWindowViewModel.ProgressText = "プロジェクトを再セットアップしています。";
                setup.Work();
            }
            else
            {
                this.IsShow = false;
            }
        }

        public void SetupOnFinished(object sender, object args)
        {
            if (sender is Setup)
            {
                var setup = (Setup)sender;
                var project = setup.Project;

                FileController.CreateDirectory(project.Path);
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();
                    writer.WritePropertyName("Project Name");
                    writer.WriteValue(project.Name);
                    writer.WritePropertyName("Project Path");
                    writer.WriteValue(project.Path);
                    writer.WritePropertyName("Minecraft Version");
                    writer.WriteValue(project.McVersion);
                    writer.WritePropertyName("Minecraft Forge Version");
                    writer.WriteValue(project.ForgeVersion);
                    writer.WritePropertyName("MCP Version");
                    writer.WriteValue(project.McpVersion);
                    writer.WriteEndObject();
                }
                FileController.CreateDirectory(Define.ProjectDirectory + "\\" + project.Name + "\\project");
                var s = new StreamWriter(Define.ProjectDirectory + "\\" + project.Name + "\\project\\settings.json");
                s.Write(sb.ToString());
                s.Close();
                s.Dispose();

                this.MainWindowViewModel.CurrentProject = project;
            }
            this.IsShow = false;
            this.ProgressWindowViewModel.IsShow = false;
        }
        #endregion


        #region EditButtonCommand
        private ViewModelCommand _EditButtonCommand;

        public ViewModelCommand EditButtonCommand
        {
            get
            {
                if (_EditButtonCommand == null)
                {
                    _EditButtonCommand = new ViewModelCommand(EditButton);
                }
                return _EditButtonCommand;
            }
        }

        public void EditButton()
        {
            var taskDialog = new TaskDialog();
            taskDialog.Caption = "警告";
            taskDialog.InstructionText = "プロジェクトの設定を変更しようとしています。";
            taskDialog.Text = "プロジェクトの基本設定を変更しようとしています。\n設定を変更した場合、うまく動作しない場合がありますが、それでもよろしいですか？";
            taskDialog.Icon = TaskDialogStandardIcon.Warning;
            taskDialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
            taskDialog.Opened += (_, __) =>
            {
                var sender = (TaskDialog)_;
                sender.Icon = sender.Icon;
            };
            taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
            taskDialog.OwnerWindowHandle = this.MainWindowViewModel.WindowHandle;
            taskDialog.Cancelable = false;
            if (taskDialog.Show() == TaskDialogResult.No)
            {
                return;
            }
            this.TextThickness = new System.Windows.Thickness(1);
            this.IsEditable = true;
        }
        #endregion


        #region IsShow変更通知プロパティ
        private bool _IsShow;

        public bool IsShow
        {
            get
            { return _IsShow; }
            set
            { 
                if (_IsShow == value)
                    return;
                _IsShow = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectDir変更通知プロパティ
        private string _ProjectDir;

        public string ProjectDir
        {
            get
            { return _ProjectDir; }
            set
            { 
                if (_ProjectDir == value)
                    return;
                _ProjectDir = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftVersion変更通知プロパティ
        private string _MinecraftVersion;

        public string MinecraftVersion
        {
            get
            { return _MinecraftVersion; }
            set
            { 
                if (_MinecraftVersion == value)
                    return;
                _MinecraftVersion = value;
                RaisePropertyChanged();

                this.MinecraftForgeVer = "";
                this.MinecraftForgeVers = Minecraft.ForgeVersions[this.MinecraftVersion];
            }
        }
        #endregion


        #region MinecraftVersions変更通知プロパティ
        private List<string> _MinecraftVersions;

        public List<string> MinecraftVersions
        {
            get
            { return _MinecraftVersions; }
            set
            { 
                if (_MinecraftVersions == value)
                    return;
                _MinecraftVersions = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftForgeVer変更通知プロパティ
        private string _MinecraftForgeVer;

        public string MinecraftForgeVer
        {
            get
            { return _MinecraftForgeVer; }
            set
            {
                if (_MinecraftForgeVer == value)
                    return;
                _MinecraftForgeVer = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftForgeVers変更通知プロパティ
        private List<string> _MinecraftForgeVers;

        public List<string> MinecraftForgeVers
        {
            get
            { return _MinecraftForgeVers; }
            set
            { 
                if (_MinecraftForgeVers == value)
                    return;
                _MinecraftForgeVers = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectName変更通知プロパティ
        private string _ProjectName;

        public string ProjectName
        {
            get
            { return _ProjectName; }
            set
            { 
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsEditable変更通知プロパティ
        private bool _IsEditable;

        public bool IsEditable
        {
            get
            { return _IsEditable; }
            set
            { 
                if (_IsEditable == value)
                    return;
                _IsEditable = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region TextThickness変更通知プロパティ
        private System.Windows.Thickness _TextThickness;

        public System.Windows.Thickness TextThickness
        {
            get
            { return _TextThickness; }
            set
            { 
                if (_TextThickness == value)
                    return;
                _TextThickness = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
